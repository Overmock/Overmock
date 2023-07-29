﻿using Lokad.ILPack;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Runtime.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    internal class InterfaceProxyMarshaller : ProxyMarshaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="argsProvider"></param>
        internal InterfaceProxyMarshaller(IOvermock target, Action<SetupArgs>? argsProvider) : base(target, argsProvider)
        {
            Name = GetName(Target);
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyName(Target), AssemblyBuilderAccess.Run
            );

            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);
            ProxyType = Constants.ProxyType.MakeGenericType(Target.Type);
        }

        /// <summary>
        /// 
        /// </summary>
        private AssemblyBuilder DynamicAssembly { get; }

        /// <summary>
        /// 
        /// </summary>
        private ModuleBuilder DynamicModule { get; }

        /// <summary>
        /// 
        /// </summary>
        private string Name { get; }

        /// <summary>
        /// 
        /// </summary>
        private Type ProxyType { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override object MarshalCore(IMarshallerContext marshallerContext)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var context = (MarshallerContext)marshallerContext;

            ImplementConstructor(context, ProxyType.GetConstructor(bindingFlags, new Type[] { Constants.OvermockType })!);

            var overmockedMethods = Target.GetOvermockedMethods().ToList();
            var methodOverrides = overmockedMethods.Select(m =>
                m.Method.IsGenericMethod
                    ? m.Method.GetGenericMethodDefinition()
                    : m.Method
            ).ToArray();

            (var methods, var properties) = AddInterfaceImplementations(context);

            methods = methods.Concat(GetBaseMethods(context))
                .Where(m => !methodOverrides.Contains(m))
                .Distinct();

            ImplementMethods(context, overmockedMethods, methods);

            var overmockedProperties = Target.GetOvermockedProperties().ToList();
            var propertyOverrides = overmockedProperties.Select(m => m.Expression.Member).ToArray();

            properties = properties
                .Where(p => !propertyOverrides.Contains(p))
                .Distinct();

            ImplementProperties(context, overmockedProperties, properties);

            var dynamicType = context.TypeBuilder.CreateType();

            // Write the assembly to disc for testing
            if (Debugger.IsAttached)
            {
                WriteAssembly();
            }
            // Write the assembly to disc for testing

            var instance = Activator.CreateInstance(dynamicType, Target)!;

            ((IProxy)instance).InitializeOvermockContext(context.OvermockContext);

            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IMarshallerContext CreateContext()
        {
            const TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;

            var typeBuilder = DynamicModule.DefineType(Constants.AssemblyAndTypeNameFormat.ApplyFormat(Target.TypeName), attributes, ProxyType);

            return new MarshallerContext(Target, typeBuilder);
        }

        private void WriteAssembly()
        {
            var generator = new AssemblyGenerator();
            var fileName = DynamicAssembly.GetName().Name!;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllBytes(fileName, generator.GenerateAssemblyBytes(DynamicAssembly));
        }

        private static void ImplementConstructor(MarshallerContext context, ConstructorInfo baseConstructor)
        {
            const MethodAttributes methodAttributes = MethodAttributes.Public;

            var callingConvention = baseConstructor.CallingConvention;
            var parameterTypes = baseConstructor.GetParameters().Select(p => p.ParameterType).ToList();

            var constructorBuilder = context.TypeBuilder.DefineConstructor(methodAttributes, callingConvention, parameterTypes.ToArray());

            var ilGenerator = constructorBuilder.GetILGenerator();

            ilGenerator.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < parameterTypes.Count; i++)
            {
                ilGenerator.Emit(OpCodes.Ldarg, i + 1);
            }

            ilGenerator.Emit(OpCodes.Call, baseConstructor);
            ilGenerator.Emit(OpCodes.Ret);
        }

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddInterfaceImplementations(MarshallerContext context)
        {
            if (!context.Target.IsInterface())
            {
                throw new OvermockException(Ex.Message.NotAnInterfaceType(context.Target));
            }

            return AddMembersRecursive(context, context.Target.Type);
        }

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddMembersRecursive(MarshallerContext context, Type interfaceType)
        {
            context.TypeBuilder.AddInterfaceImplementation(interfaceType);

            context.AddInterfaces(interfaceType);

            var methods = interfaceType.GetMethods().AsEnumerable();
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(context, type).Concat(methods);
                properties = AddPropertiesRecursive(context, type).Concat(properties);
            }

            return (methods.Distinct(), properties.Distinct());
        }

        private static IEnumerable<MethodInfo> AddMethodsRecursive(MarshallerContext context, Type interfaceType)
        {
            var methods = interfaceType.GetMethods().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(context, type).Concat(methods);
            }

            return methods.Distinct();
        }

        private static IEnumerable<PropertyInfo> AddPropertiesRecursive(MarshallerContext context, Type interfaceType)
        {
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                properties = AddPropertiesRecursive(context, type).Concat(properties);
            }

            return properties.Distinct();
        }

        private static IEnumerable<MethodInfo> GetBaseMethods(MarshallerContext context)
        {
            if (context.Target.IsDelegate())
            {
                return new[] { context.Target.Type.GetMethod(Constants.InvokeMethodName)! };
            }

            return typeof(object).GetMethods().Where(method => method.IsVirtual);
        }

        private static void DefineOvermockInitMethod(TypeBuilder typeBuilder, FieldBuilder contextField)
        {
            var initContextMethod = typeBuilder.DefineMethod(Constants.InitializeOvermockContextMethodName,
                MethodAttributes.Public, CallingConventions.HasThis, typeof(void),
                new[] { contextField.FieldType }
            );
            var initContextMethodBody = initContextMethod.GetILGenerator();
            initContextMethodBody.Emit(OpCodes.Ldarg_0);
            initContextMethodBody.Emit(OpCodes.Ldarg_1);
            initContextMethodBody.Emit(OpCodes.Stfld, contextField);
            initContextMethodBody.Emit(OpCodes.Ret);
        }

        private static void ImplementMethods(MarshallerContext context, IEnumerable<IMethodCall> overmocks, IEnumerable<MethodInfo> methods)
        {
            foreach (var method in methods)
            {
                CreateMethod(context, method);
            }

            foreach (var mock in overmocks)
            {
                var methodBuilder = CreateMethod(context,
                    mock.Method.IsGenericMethod
                        ? mock.Method.GetGenericMethodDefinition()
                        : mock.Method
                );

                var methodId = Guid.NewGuid();
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(OvermockAttribute).GetConstructors().First(),
                    new object[] { methodId.ToString() }
                ));

                context.OvermockContext.Add(methodId, new RuntimeContext(mock,
                    mock.Method,
                    mock.Method.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
                );
            }
        }

        private static MethodBuilder CreateMethod(MarshallerContext context, MethodInfo methodInfo)
        {
            var parameterTypes = methodInfo.GetParameters()
                .Select(p => p.ParameterType).ToArray();

            var methodBuilder = context.TypeBuilder.DefineMethod(
                methodInfo.Name,
                methodInfo.IsAbstract
                    ? methodInfo.Attributes ^ MethodAttributes.Abstract
                    : methodInfo.Attributes,
                methodInfo.ReturnType,
                parameterTypes
            );

            var iLGenerator = methodBuilder.GetILGenerator();
            var returnType = methodInfo.ReturnType;

            if (methodInfo.IsGenericMethod)
            {
                DefineGenericParameters(methodInfo, methodBuilder);
            }

            EmitMethodBody(context, iLGenerator, returnType, parameterTypes);

            return methodBuilder;
        }

        private static void DefineGenericParameters(MethodInfo methodInfo, MethodBuilder methodBuilder)
        {
            var genericParameters = methodInfo.GetGenericArguments();
            var genericParameterBuilders = methodBuilder.DefineGenericParameters(genericParameters.Select(t => t.Name).ToArray());

            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var baseGenericArgument = genericParameters[i];
                var genericParameterBuilder = genericParameterBuilders[i];

                genericParameterBuilder.SetGenericParameterAttributes(baseGenericArgument.GenericParameterAttributes);

                foreach (var baseTypeConstraint in baseGenericArgument.GetGenericParameterConstraints())
                {
                    if (baseTypeConstraint.IsInterface)
                    {
                        genericParameterBuilder.SetInterfaceConstraints(baseTypeConstraint);
                    }
                    else
                    {
                        genericParameterBuilder.SetBaseTypeConstraint(baseTypeConstraint);
                    }
                }
            }
        }

        private static void EmitMethodBody(MarshallerContext context, ILGenerator emitter, Type returnType, Type[] parameters)
        {
            var returnIsNotVoid = returnType != typeof(void);

            if (returnIsNotVoid)
            {
                emitter.DeclareLocal(returnType);
            }

            var returnLabel = emitter.DefineLabel();

            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Ldarg_0);

            emitter.Emit(OpCodes.Call, Constants.MethodBaseTypeGetCurrentMethod);
            emitter.Emit(OpCodes.Castclass, Constants.MethodInfoType);

            if (parameters.Length > 0)
            {
                emitter.Emit(OpCodes.Ldc_I4, parameters.Length);
                emitter.Emit(OpCodes.Newarr, Constants.ObjectType);

                for (int i = 0; i < parameters.Length - 1; i++)
                {
                    emitter.Emit(OpCodes.Dup);
                    emitter.Emit(OpCodes.Ldc_I4, i);
                    emitter.Emit(OpCodes.Ldarg, i + 1);

                    if (parameters[i].IsValueType)
                    {
                        emitter.Emit(OpCodes.Box, parameters[i]);
                    }

                    emitter.Emit(OpCodes.Stelem_Ref);
                }

                emitter.Emit(OpCodes.Dup);
                emitter.Emit(OpCodes.Ldc_I4, parameters.Length - 1);
                emitter.Emit(OpCodes.Ldarg, parameters.Length - 1);
                emitter.Emit(OpCodes.Stelem_Ref);
            }
            else
            {
                emitter.EmitCall(OpCodes.Call, Constants.EmptyObjectArrayMethod, null);
            }

            emitter.EmitCall(OpCodes.Callvirt, Constants.GetProxyTypeHandleMethodCallMethod(context.Target.Type), null);

            if (returnIsNotVoid)
            {
                if (returnType.IsValueType)
                {
                    emitter.Emit(OpCodes.Unbox_Any, returnType);
                }
                else
                {
                    emitter.Emit(OpCodes.Castclass, returnType);

                    emitter.Emit(OpCodes.Stloc_0);
                    emitter.Emit(OpCodes.Br_S, returnLabel);

                    emitter.MarkLabel(returnLabel);
                    emitter.Emit(OpCodes.Ldloc_0);
                }
            }
            else
            {
                emitter.Emit(OpCodes.Pop);
            }

            emitter.Emit(OpCodes.Ret);
        }

        private static void ImplementProperties(MarshallerContext context, IEnumerable<IPropertyCall> overmocks, IEnumerable<PropertyInfo> properties)
        {
            foreach (var method in properties)
            {
                CreateMethod(context, method.GetGetMethod()!);
            }

            foreach (var mock in overmocks)
            {
                var propertyGetter = mock.PropertyInfo.GetGetMethod();
                var methodBuilder = CreateMethod(context, propertyGetter!);

                var methodId = Guid.NewGuid();
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    typeof(OvermockAttribute).GetConstructors().First(),
                    new object[] { methodId.ToString() }
                ));

                context.OvermockContext.Add(methodId, new RuntimeContext(mock,
                    propertyGetter,
                    Enumerable.Empty<RuntimeParameter>())
                );
            }
        }

        private static void DefineProperties(MarshallerContext context)
        {
            foreach (PropertyInfo property in context.Target.Type.GetProperties())
            {
                CreateProperty(context, property);
            }
        }

        private static TypeBuilder CreateProperty(MarshallerContext context, PropertyInfo propertyInfo)
        {
            var typeBuilder = context.TypeBuilder;

            PropertyBuilder property = typeBuilder.DefineProperty(propertyInfo.Name, PropertyAttributes.HasDefault, CallingConventions.HasThis, propertyInfo.PropertyType, new Type[1]
            {
                propertyInfo.PropertyType
            });

            FieldBuilder field = typeBuilder.DefineField("_" + propertyInfo.Name.ToLower(), propertyInfo.PropertyType, FieldAttributes.Private);

            if (propertyInfo.CanRead)
            {
                CreateGetMethod(context, property, propertyInfo.GetGetMethod()!, field, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName);
            }

            //if (propertyInfo.CanWrite)
            //{
            //    CreateSetMethod(typeBuilder, property, propertyInfo.GetSetMethod()!, field, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.SpecialName);
            //}

            return typeBuilder;
        }

        private static void CreateGetMethod(
            MarshallerContext context,
            PropertyBuilder property,
            MethodInfo method,
            FieldInfo field,
            MethodAttributes getAttrs)
        {
            MethodBuilder mdBuilder = context.TypeBuilder.DefineMethod(method.Name, getAttrs, method.ReturnType, Type.EmptyTypes);

            EmitMethodBody(context, mdBuilder.GetILGenerator(), property.PropertyType, Type.EmptyTypes);

            //ILGenerator ilGenerator = mdBuilder.GetILGenerator();
            //ilGenerator.Emit(OpCodes.Ldarg_0);
            //ilGenerator.Emit(OpCodes.Ldfld, field);
            //ilGenerator.Emit(OpCodes.Ret);

            property.SetGetMethod(mdBuilder);
        }

        private static void CreateSetMethod(
            TypeBuilder baseType,
            PropertyBuilder property,
            MethodInfo method,
            FieldInfo field,
            MethodAttributes setAttrs)
        {
            MethodBuilder mdBuilder = baseType.DefineMethod(method.Name, setAttrs, method.ReturnType, new Type[1]
            {
        field.FieldType
            });
            ILGenerator ilGenerator = mdBuilder.GetILGenerator();
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Stfld, field);
            ilGenerator.Emit(OpCodes.Ret);
            property.SetSetMethod(mdBuilder);
        }

        private static string GetDynamicTypeName(Type baseType, string prefix = "", string suffix = "") =>
            $"{prefix}{baseType.FullName}{suffix}";

        private class MarshallerContext : IMarshallerContext
        {
            public MarshallerContext(IOvermock target, TypeBuilder typeBuilder)
            {
                Target = target;
                TypeBuilder = typeBuilder;
                Interfaces = new List<Type>();
                OvermockContext = new ProxyOverrideContext();
            }

            public IOvermock Target { get; }

            public TypeBuilder TypeBuilder { get; }

            public ProxyOverrideContext OvermockContext { get; }

            private List<Type> Interfaces { get; set; }

            public void AddInterfaces(params Type[] interfaceTypes)
            {
                var interfaces = Interfaces.ToArray();

                Interfaces = interfaces.Union(interfaceTypes).Distinct().ToList();
            }
        }
    }
}