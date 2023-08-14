using System.Reflection;
using System.Reflection.Emit;
using Kimono.Proxies;

namespace Kimono.Internal
{
	/// <summary>
	/// Class InterfaceProxyFactory.
	/// Implements the <see cref="Proxies.ProxyFactory" />
	/// </summary>
	/// <seealso cref="Proxies.ProxyFactory" />
	internal sealed class InterfaceProxyFactory : ProxyFactory
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="InterfaceProxyFactory" /> class.
		/// </summary>
		/// <param name="cache">The cache.</param>
		/// <param name="methodGenerator">The method generator.</param>
		/// <param name="propertyGenerator">The property generator.</param>
		internal InterfaceProxyFactory(IProxyCache cache, IProxyMethodGenerator? methodGenerator = null, IProxyPropertyGenerator? propertyGenerator = null) : base(cache)
        {
            Name = GetName(Constants.AssemblyId);
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyName(Constants.AssemblyId), AssemblyBuilderAccess.Run
            );

            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);

			MethodGenerator = methodGenerator ?? new ProxyMethodGenerator();
			PropertyGenerator = propertyGenerator ?? new ProxyPropertyGenerator();
        }

		private AssemblyBuilder DynamicAssembly { get; }

		private ModuleBuilder DynamicModule { get; }

		private IProxyMethodGenerator MethodGenerator { get; }

		private IProxyPropertyGenerator PropertyGenerator { get; }

		private string Name { get; }

		protected override IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext builderContext)
        {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            var context = (ProxyBuilderContext)builderContext;

            ImplementConstructor(context, context.ProxyType.GetConstructor(bindingFlags, Type.EmptyTypes)!);

            (var methods, var properties) = AddInterfaceImplementations(context);
			properties = properties.Distinct();
			methods = methods.Concat(GetBaseMethods(context))
                .Distinct();

			MethodGenerator.Generate(context, methods);
            PropertyGenerator.Generate(context, properties);

            var proxyType = context.TypeBuilder.CreateType();

            return Cache.SetGenerator(
				context.Interceptor.TargetType,
				new ProxyGenerator<T>(proxyType, context.ProxyContext, CreateProxy)
			);
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
                throw new KimonoException($"Type must be an interface: {context.Interceptor}");
            }

            return AddMembersRecursive(context, context.Interceptor.TargetType);
        }

		private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddMembersRecursive(ProxyBuilderContext context, Type interfaceType)
		{
			context.TypeBuilder.AddInterfaceImplementation(interfaceType);

			context.AddInterfaces(interfaceType);

			var methods = GetMethods(interfaceType);
			var properties = interfaceType.GetProperties().AsEnumerable();

			foreach (Type type in interfaceType.GetInterfaces())//.Where(i => i != Constants.DisposableType)
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

		private static object CreateProxy(ProxyContext context, IInterceptor interceptor, Type proxyType)
		{
			var instance = Activator.CreateInstance(proxyType)!;

			((IProxy)instance).InitializeProxyContext(interceptor, context);

			return instance;
		}
	}
}