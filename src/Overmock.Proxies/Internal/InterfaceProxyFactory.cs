using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Proxies.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal class InterfaceProxyFactory : ProxyFactory
    {
		private static readonly ConstructorInfo OvermockAttributeConstructor = typeof(OvermockAttribute).GetConstructors().First();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="interceptor"></param>
		/// <param name="argsProvider"></param>
		internal InterfaceProxyFactory(IProxyCache cache) : base(cache)
        {
            Name = GetName(Constants.AssemblyId);
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyName(Constants.AssemblyId), AssemblyBuilderAccess.Run
            );

            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);
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
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        protected override IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext marshallerContext)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var context = (ProxyBuilderContext)marshallerContext;

            ImplementConstructor(context, context.ProxyType.GetConstructor(bindingFlags, Type.EmptyTypes)!);

            (var methods, var properties) = AddInterfaceImplementations(context);

            methods = methods.Concat(GetBaseMethods(context))
                .Distinct();

            ImplementMethods(context, methods);

            properties = properties.Distinct();

            ImplementProperties(context, properties);

            var dynamicType = context.TypeBuilder.CreateType();

            // Write the assembly to disc for testing
            if (Debugger.IsAttached)
            {
                WriteAssembly();
            }
            // Write the assembly to disc for testing

            static object CreateProxy(ProxyContext context, IInterceptor interceptor, Type dynamicType)
			{
				var instance = Activator.CreateInstance(dynamicType)!;

				((IProxy)instance).InitializeProxyContext(interceptor, context);

				return instance;
			}

            var proxyGenerator = new ProxyGenerator<T>(context.ProxyContext, dynamicType, CreateProxy);

			Cache.Set(context.Interceptor.TargetType, proxyGenerator);

            return proxyGenerator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override IProxyBuilderContext CreateContext(IInterceptor interceptor)
        {
            const TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;

            var proxyType = Constants.ProxyType.MakeGenericType(interceptor.TargetType);

			var typeBuilder = DynamicModule.DefineType(Constants.AssemblyAndTypeNameFormat.ApplyFormat(interceptor.TypeName), attributes, proxyType);

            return new ProxyBuilderContext(interceptor, typeBuilder, proxyType);
        }

        private void WriteAssembly()
        {
            var generator = new Lokad.ILPack.AssemblyGenerator();
            var fileName = DynamicAssembly.GetName().Name!;

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllBytes(fileName, generator.GenerateAssemblyBytes(DynamicAssembly));
        }

        private static void ImplementConstructor(ProxyBuilderContext context, ConstructorInfo baseConstructor)
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

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddInterfaceImplementations(ProxyBuilderContext context)
        {
            if (!context.Interceptor.IsInterface())
            {
                throw new OvermockException($"Type must be an interface: {context.Interceptor}");
            }

            return AddMembersRecursive(context, context.Interceptor.TargetType);
        }

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddMembersRecursive(ProxyBuilderContext context, Type interfaceType)
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

        private static IEnumerable<MethodInfo> AddMethodsRecursive(ProxyBuilderContext context, Type interfaceType)
        {
            var methods = interfaceType.GetMethods().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(context, type).Concat(methods);
            }

            return methods.Distinct();
        }

        private static IEnumerable<PropertyInfo> AddPropertiesRecursive(ProxyBuilderContext context, Type interfaceType)
        {
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                properties = AddPropertiesRecursive(context, type).Concat(properties);
            }

            return properties.Distinct();
        }

        private static IEnumerable<MethodInfo> GetBaseMethods(ProxyBuilderContext context)
        {
            if (context.Interceptor.IsDelegate())
            {
                return new[] { context.Interceptor.TargetType.GetMethod(Constants.InvokeMethodName)! };
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

        private static void ImplementMethods(ProxyBuilderContext context, IEnumerable<MethodInfo> methods)
        {
            foreach (var methodInfo in methods)
            {
                var method = new ProxyMember(methodInfo);
                var methodBuilder = CreateMethod(context,
					methodInfo.IsGenericMethod
                        ? methodInfo.GetGenericMethodDefinition()
                        : methodInfo
				);

                var methodId = Guid.NewGuid();
                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    OvermockAttributeConstructor,
                    new object[] { methodId.ToString() }
                ));

                context.ProxyContext.Add(methodId, new RuntimeContext(method,
					methodInfo.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
                );
            }
        }

        private static MethodBuilder CreateMethod(ProxyBuilderContext context, MethodInfo methodInfo)
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

        private static void EmitMethodBody(ProxyBuilderContext context, ILGenerator emitter, Type returnType, Type[] parameters)
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
                emitter.Emit(OpCodes.Ldarg, parameters.Length);
                emitter.Emit(OpCodes.Stelem_Ref);
            }
            else
            {
                emitter.EmitCall(OpCodes.Call, Constants.EmptyObjectArrayMethod, null);
            }

            emitter.EmitCall(OpCodes.Call, Constants.GetProxyTypeHandleMethodCallMethod(context.Interceptor.TargetType), null);

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

        private static void ImplementProperties(ProxyBuilderContext context, IEnumerable<PropertyInfo> properties)
        {
            foreach (var propertyInfo in properties)
			{
                if (propertyInfo.CanRead)
				{
					var property = new ProxyMember(propertyInfo, propertyInfo.GetGetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method);

					var methodId = Guid.NewGuid();
					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                        OvermockAttributeConstructor,
						new object[] { methodId.ToString() }
					));

					context.ProxyContext.Add(methodId, new RuntimeContext(
                        property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
				if (propertyInfo.CanWrite)
				{
					var property = new ProxyMember(propertyInfo, propertyInfo.GetSetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method);

					var methodId = Guid.NewGuid();
					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
						OvermockAttributeConstructor,
						new object[] { methodId.ToString() }
					));

					context.ProxyContext.Add(methodId, new RuntimeContext(
						property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
			}
        }

        private static void DefineProperties(ProxyBuilderContext context)
        {
            var targetType = context.Interceptor.TargetType;

            foreach (PropertyInfo property in targetType.GetProperties())
            {
                CreateProperty(context, property);
            }
        }

        private static TypeBuilder CreateProperty(ProxyBuilderContext context, PropertyInfo propertyInfo)
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
            ProxyBuilderContext context,
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

        private class ProxyBuilderContext : IProxyBuilderContext
		{
            public ProxyBuilderContext(IInterceptor target, TypeBuilder typeBuilder, Type proxyType)
            {
                Interceptor = target;
                ProxyType = proxyType;
                TypeBuilder = typeBuilder;
                Interfaces = new List<Type>();
                ProxyContext = new ProxyContext();
            }

            public IInterceptor Interceptor { get; }

			public Type ProxyType { get; }
			
            public TypeBuilder TypeBuilder { get; }

            public ProxyContext ProxyContext { get; }

            private List<Type> Interfaces { get; set; }

            public void AddInterfaces(params Type[] interfaceTypes)
            {
                var interfaces = Interfaces.ToArray();

                Interfaces = interfaces.Union(interfaceTypes).Distinct().ToList();
            }
        }
    }
}