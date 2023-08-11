using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
	/// <summary>
	/// Class InterfaceProxyFactory.
	/// Implements the <see cref="Kimono.ProxyFactory" />
	/// </summary>
	/// <seealso cref="Kimono.ProxyFactory" />
	internal class InterfaceProxyFactory : ProxyFactory
    {
		/// <summary>
		/// The kimono attribute constructor
		/// </summary>
		private static readonly ConstructorInfo KimonoAttributeConstructor = typeof(KimonoAttribute).GetConstructors().First();

		/// <summary>
		/// Initializes a new instance of the <see cref="InterfaceProxyFactory"/> class.
		/// </summary>
		/// <param name="cache">The cache.</param>
		internal InterfaceProxyFactory(IProxyCache cache) : base(cache)
        {
            Name = GetName(Constants.AssemblyId);
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyName(Constants.AssemblyId), AssemblyBuilderAccess.Run
            );

            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);
        }

		/// <summary>
		/// Gets the dynamic assembly.
		/// </summary>
		/// <value>The dynamic assembly.</value>
		private AssemblyBuilder DynamicAssembly { get; }

		/// <summary>
		/// Gets the dynamic module.
		/// </summary>
		/// <value>The dynamic module.</value>
		private ModuleBuilder DynamicModule { get; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		private string Name { get; }

		/// <summary>
		/// Creates the core.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="builderContext">The builder context.</param>
		/// <returns>IProxyGenerator&lt;T&gt;.</returns>
		/// <exception cref="NotImplementedException"></exception>
		protected override IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext builderContext)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var context = (ProxyBuilderContext)builderContext;

            ImplementConstructor(context, context.ProxyType.GetConstructor(bindingFlags, Type.EmptyTypes)!);

            (var methods, var properties) = AddInterfaceImplementations(context);

            methods = methods.Concat(GetBaseMethods(context))
                .Distinct();

            ImplementMethods(context, methods);

            properties = properties.Distinct();

            ImplementProperties(context, properties);

            var dynamicType = context.TypeBuilder.CreateType();

//#if DEBUG

//            //Write the assembly to disc for testing
//            if (Debugger.IsAttached)
//            {
//                WriteAssembly();
//            }
//            //Write the assembly to disc for testing

//#endif

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
		/// Creates the context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyBuilderContext.</returns>
		protected override IProxyBuilderContext CreateContext(IInterceptor interceptor)
        {
            const TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;

            var proxyType = Constants.ProxyType.MakeGenericType(interceptor.TargetType);

			var typeBuilder = DynamicModule.DefineType(Constants.AssemblyAndTypeNameFormat.ApplyFormat(interceptor.TypeName), attributes, proxyType);

            return new ProxyBuilderContext(interceptor, typeBuilder, proxyType);
        }

		// #if DEBUG
		//        private void WriteAssembly()
		//        {
		//            var generator = new Lokad.ILPack.AssemblyGenerator();
		//            var fileName = DynamicAssembly.GetName().Name!;

		//            if (File.Exists(fileName))
		//            {
		//                File.Delete(fileName);
		//            }

		//            File.WriteAllBytes(fileName, generator.GenerateAssemblyBytes(DynamicAssembly));
		//        }
		//#endif

		/// <summary>
		/// Implements the constructor.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="baseConstructor">The base constructor.</param>
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

		/// <summary>
		/// Adds the interface implementations.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.ValueTuple&lt;IEnumerable&lt;MethodInfo&gt;, IEnumerable&lt;PropertyInfo&gt;&gt;.</returns>
		/// <exception cref="Kimono.KimonoException">Type must be an interface: {context.Interceptor}</exception>
		private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddInterfaceImplementations(ProxyBuilderContext context)
        {
            if (!context.Interceptor.IsInterface())
            {
                throw new KimonoException($"Type must be an interface: {context.Interceptor}");
            }

            return AddMembersRecursive(context, context.Interceptor.TargetType);
        }

		/// <summary>
		/// Adds the members recursive.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>System.ValueTuple&lt;IEnumerable&lt;MethodInfo&gt;, IEnumerable&lt;PropertyInfo&gt;&gt;.</returns>
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

		/// <summary>
		/// Adds the methods recursive.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
		private static IEnumerable<MethodInfo> AddMethodsRecursive(ProxyBuilderContext context, Type interfaceType)
        {
            var methods = GetMethods(interfaceType);

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(context, type).Concat(methods);
            }

            return methods.Distinct();
		}

		/// <summary>
		/// Gets the methods.
		/// </summary>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
		private static IEnumerable<MethodInfo> GetMethods(Type interfaceType)
		{
			return interfaceType.GetMethods().Where(m => !m.IsSpecialName);
		}

		/// <summary>
		/// Adds the properties recursive.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="interfaceType">Type of the interface.</param>
		/// <returns>IEnumerable&lt;PropertyInfo&gt;.</returns>
		private static IEnumerable<PropertyInfo> AddPropertiesRecursive(ProxyBuilderContext context, Type interfaceType)
        {
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                properties = AddPropertiesRecursive(context, type).Concat(properties);
            }

            return properties.Distinct();
        }

		/// <summary>
		/// Gets the base methods.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>IEnumerable&lt;MethodInfo&gt;.</returns>
		private static IEnumerable<MethodInfo> GetBaseMethods(ProxyBuilderContext context)
        {
            if (context.Interceptor.IsDelegate())
            {
                return new[] { context.Interceptor.TargetType.GetMethod(Constants.InvokeMethodName)! };
            }

            return typeof(object).GetMethods().Where(method => method.IsVirtual);
        }

		/// <summary>
		/// Implements the methods.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methods">The methods.</param>
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
                    KimonoAttributeConstructor,
                    new object[] { methodId }
                ));

                context.ProxyContext.Add(methodId, new RuntimeContext(method,
					methodInfo.GetParameters().Select(p => new RuntimeParameter(p.Name!, type: p.ParameterType)))
                );
            }
        }

		/// <summary>
		/// Creates the method.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methodInfo">The method information.</param>
		/// <param name="methodId">The method identifier.</param>
		/// <returns>MethodBuilder.</returns>
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

		/// <summary>
		/// Defines the generic parameters.
		/// </summary>
		/// <param name="methodInfo">The method information.</param>
		/// <param name="methodBuilder">The method builder.</param>
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

		/// <summary>
		/// Emits the method body.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="emitter">The emitter.</param>
		/// <param name="returnType">Type of the return.</param>
		/// <param name="parameters">The parameters.</param>
		/// <param name="methodId">The method identifier.</param>
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

		/// <summary>
		/// Implements the properties.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="properties">The properties.</param>
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
                        KimonoAttributeConstructor,
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
						KimonoAttributeConstructor,
						new object[] { methodId.ToString() }
					));

					context.ProxyContext.Add(methodId, new RuntimeContext(
						property,
						Enumerable.Empty<RuntimeParameter>())
					);
				}
			}
        }

		/// <summary>
		/// Class ProxyBuilderContext.
		/// Implements the <see cref="Kimono.ProxyFactory.IProxyBuilderContext" />
		/// </summary>
		/// <seealso cref="Kimono.ProxyFactory.IProxyBuilderContext" />
		private class ProxyBuilderContext : IProxyBuilderContext
		{
			/// <summary>
			/// The method counter
			/// </summary>
			private int _methodCounter = 0;
			/// <summary>
			/// Initializes a new instance of the <see cref="ProxyBuilderContext"/> class.
			/// </summary>
			/// <param name="target">The target.</param>
			/// <param name="typeBuilder">The type builder.</param>
			/// <param name="proxyType">Type of the proxy.</param>
			public ProxyBuilderContext(IInterceptor target, TypeBuilder typeBuilder, Type proxyType)
            {
                Interceptor = target;
                ProxyType = proxyType;
                TypeBuilder = typeBuilder;
                Interfaces = new List<Type>();
                ProxyContext = new ProxyContext();
            }

			/// <summary>
			/// Gets the interceptor.
			/// </summary>
			/// <value>The interceptor.</value>
			public IInterceptor Interceptor { get; }

			/// <summary>
			/// Gets the type of the proxy.
			/// </summary>
			/// <value>The type of the proxy.</value>
			public Type ProxyType { get; }

			/// <summary>
			/// Gets the type builder.
			/// </summary>
			/// <value>The type builder.</value>
			public TypeBuilder TypeBuilder { get; }

			/// <summary>
			/// Gets the proxy context.
			/// </summary>
			/// <value>The proxy context.</value>
			public ProxyContext ProxyContext { get; }

			/// <summary>
			/// Gets or sets the interfaces.
			/// </summary>
			/// <value>The interfaces.</value>
			private List<Type> Interfaces { get; set; }

			/// <summary>
			/// Adds the interfaces.
			/// </summary>
			/// <param name="interfaceTypes">The interface types.</param>
			public void AddInterfaces(params Type[] interfaceTypes)
            {
                var interfaces = Interfaces.ToArray();

                Interfaces = interfaces.Union(interfaceTypes).Distinct().ToList();
            }

			/// <summary>
			/// Gets the next method identifier.
			/// </summary>
			/// <returns>System.Int32.</returns>
			public int GetNextMethodId()
            {
                return _methodCounter++;
            }
        }
    }
}