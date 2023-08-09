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
            //if (Debugger.IsAttached)
            //{
            //    WriteAssembly();
            //}
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

			var methods = GetMethods(interfaceType);
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
            var methods = GetMethods(interfaceType);

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(context, type).Concat(methods);
            }

            return methods.Distinct();
		}

		private static IEnumerable<MethodInfo> GetMethods(Type interfaceType)
		{
			return interfaceType.GetMethods().Where(m => !m.IsSpecialName);
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

        private static void ImplementMethods(ProxyBuilderContext context, IEnumerable<MethodInfo> methods)
        {
            foreach (var methodInfo in methods)
			{
                var methodId = context.GetNextMethodId();
				var method = new ProxyMember(methodInfo);
                var methodBuilder = CreateMethod(context,
					methodInfo.IsGenericMethod
                        ? methodInfo.GetGenericMethodDefinition()
                        : methodInfo,
                    methodId
				);

                methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                    OvermockAttributeConstructor,
                    new object[] { methodId }
                ));

                context.ProxyContext.Add(methodId, new RuntimeContext(method,
					methodInfo.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
                );
            }
        }

        private static MethodBuilder CreateMethod(ProxyBuilderContext context, MethodInfo methodInfo, int methodId)
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

            if (methodInfo.DeclaringType == Constants.ObjectType)
            {
                context.TypeBuilder.DefineMethodOverride(methodBuilder,  methodInfo);
            }

            var iLGenerator = methodBuilder.GetILGenerator();
            var returnType = methodInfo.ReturnType;

            if (methodInfo.IsGenericMethod)
            {
                DefineGenericParameters(methodInfo, methodBuilder);
            }

            EmitMethodBody(context, iLGenerator, returnType, parameterTypes, methodId);

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

        private static void EmitMethodBody(ProxyBuilderContext context, ILGenerator emitter, Type returnType, Type[] parameters, int methodId)
        {
            var returnIsNotVoid = returnType != typeof(void);

            if (returnIsNotVoid)
            {
                emitter.DeclareLocal(returnType);
            }

            var returnLabel = emitter.DefineLabel();

            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Ldarg_0);

            emitter.Emit(OpCodes.Ldc_I4, methodId);

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
					var methodId = context.GetNextMethodId();
					var property = new ProxyMember(propertyInfo, propertyInfo.GetGetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method, methodId);

					methodBuilder.SetCustomAttribute(new CustomAttributeBuilder(
                        OvermockAttributeConstructor,
						new object[] { methodId }
					));

					context.ProxyContext.Add(methodId, new RuntimeContext(
                        property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
				if (propertyInfo.CanWrite)
				{
					var methodId = context.GetNextMethodId();
					var property = new ProxyMember(propertyInfo, propertyInfo.GetSetMethod()!);
					var methodBuilder = CreateMethod(context, property.Method, methodId);

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

        private class ProxyBuilderContext : IProxyBuilderContext
		{
            private int _methodCounter = 0;
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

            public int GetNextMethodId()
            {
                return _methodCounter++;
            }
        }
    }
}