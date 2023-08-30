using Kimono.Emit;
using Kimono.Proxies;
using Kimono.Proxies.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    /// <summary>
    /// Class InterfaceProxyFactory.
    /// Implements the <see cref="ProxyFactory" />
    /// </summary>
    /// <seealso cref="Proxies.ProxyFactory" />
    internal sealed class InterfaceProxyFactory : ProxyFactory
    {
        private static readonly ParameterExpression[] _proxyDelegateParameterExpressions = new ParameterExpression[] {
            Expression.Parameter(Constants.ProxyContextType),
            Expression.Parameter(Constants.IInterceptorType)
        };
        private static readonly Type[] _proxyDelegateParameters = new Type[] {
            Constants.ProxyContextType,
            Constants.IInterceptorType
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="InterfaceProxyFactory" /> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        /// <param name="methodFactory">The method factory.</param>
        /// <param name="propertyFactory">The property factory.</param>
        internal InterfaceProxyFactory(IProxyCache cache, IProxyMethodFactory? methodFactory = null, IProxyPropertyFactory? propertyFactory = null) : base(cache)
        {
            Name = GetName(Constants.AssemblyId);
            DynamicAssembly = AssemblyBuilder.DefineDynamicAssembly(
                GetAssemblyName(Constants.AssemblyId), AssemblyBuilderAccess.Run
            );

            DynamicModule = DynamicAssembly.DefineDynamicModule(Name);

            MethodFactory = methodFactory ?? new ProxyMethodFactory();
            PropertyFactory = propertyFactory ?? new ProxyPropertyFactory();
        }

        private AssemblyBuilder DynamicAssembly { get; }

        private ModuleBuilder DynamicModule { get; }

        //public IDelegateFactory DelegateFactory => FactoryProvider.DelegateFactory;

        private IProxyMethodFactory MethodFactory { get; }

        private IProxyPropertyFactory PropertyFactory { get; }

        private string Name { get; }

        protected override IProxyGenerator<T> CreateCore<T>(IProxyContextBuilder builderContext)
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            var context = (ProxyContextBuilder)builderContext;
            var constructor = context.ProxyType.GetConstructor(
                bindingFlags, null,
                Constants.ProxyBaseCtorParameterTypes, null
            )!;

            ImplementConstructor(context, constructor);

            (var methods, var properties) = AddInterfaceImplementations(context);

            MethodFactory.Create(context, methods
                .Concat(GetBaseMethods())
                .Distinct());
            PropertyFactory.Create(context, properties.Distinct());

            var proxyType = context.TypeBuilder.CreateType();
            var proxyConstructor = proxyType.GetConstructor(_proxyDelegateParameters)!;
            return Cache.SetGenerator(context.Interceptor.TargetType,
                new ProxyGenerator<T>(context.ProxyContext, CreateProxyDelegate<T>(proxyType, proxyConstructor)
            ));
        }

        /// <summary>
        /// Creates the context.
        /// </summary>
        /// <param name="interceptor">The interceptor.</param>
        /// <returns>IProxyBuilderContext.</returns>
        protected override IProxyContextBuilder CreateContext(IInterceptor interceptor)
        {
            const TypeAttributes attributes = TypeAttributes.Class | TypeAttributes.Public;

            var proxyType = Constants.ProxyType.MakeGenericType(interceptor.TargetType);
            var typeBuilder = DynamicModule.DefineType(
                ProxyName.GetName(interceptor),
                attributes,
                proxyType
            );
            return new ProxyContextBuilder(interceptor, typeBuilder, proxyType);
        }

        //private void WriteAssembly()
        //{
        //    var generator = new Lokad.ILPack.AssemblyGenerator();
        //    var fileName = Path.Combine("c://git/github/Overmock/Dynamic/", DynamicAssembly.GetName().Name!);

        //    if (File.Exists(fileName))
        //    {
        //        File.Delete(fileName);
        //    }

        //    File.WriteAllBytes(fileName, generator.GenerateAssemblyBytes(DynamicAssembly));
        //}

        private void ImplementConstructor(ProxyContextBuilder context, ConstructorInfo baseConstructor)
        {
            const MethodAttributes attributes = MethodAttributes.Public;

            var parameterTypes = baseConstructor.GetParameters()
                .Select(p => p.ParameterType)
                .ToList();

            var constructorBuilder = context.TypeBuilder.DefineConstructor(
                attributes,
                baseConstructor.CallingConvention,
                parameterTypes.ToArray()
            );

            MethodFactory.EmitConstructor(constructorBuilder.GetEmitter(), baseConstructor);
        }

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddInterfaceImplementations(ProxyContextBuilder context)
        {
            if (!context.Interceptor.IsInterface())
            {
                throw new KimonoException($"Type must be an interface: {context.Interceptor}");
            }

            return AddMembersRecursive(context, context.Interceptor.TargetType);
        }

        private static (IEnumerable<MethodInfo>, IEnumerable<PropertyInfo>) AddMembersRecursive(ProxyContextBuilder context, Type interfaceType)
        {
            context.TypeBuilder.AddInterfaceImplementation(interfaceType);

            var methods = GetMethods(interfaceType);
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(type).Concat(methods);
                properties = AddPropertiesRecursive(type).Concat(properties);
            }

            return (methods.Distinct(), properties.Distinct());
        }

        private static IEnumerable<MethodInfo> AddMethodsRecursive(Type interfaceType)
        {
            var methods = GetMethods(interfaceType);

            foreach (Type type in interfaceType.GetInterfaces())
            {
                methods = AddMethodsRecursive(type).Concat(methods);
            }

            return methods.Distinct();
        }

        private static IEnumerable<MethodInfo> GetMethods(Type interfaceType)
        {
            return interfaceType.GetMethods().Where(m => !m.IsSpecialName);
        }

        private static IEnumerable<PropertyInfo> AddPropertiesRecursive(Type interfaceType)
        {
            var properties = interfaceType.GetProperties().AsEnumerable();

            foreach (Type type in interfaceType.GetInterfaces())
            {
                properties = AddPropertiesRecursive(type).Concat(properties);
            }

            return properties.Distinct();
        }

        private static IEnumerable<MethodInfo> GetBaseMethods()
        {
            return typeof(object).GetMethods().Where(method => method.IsVirtual);
        }

        //private static Func<ProxyContext, IInterceptor, T> CreateProxyDelegate<T>(Type proxyType, ConstructorInfo proxyConstructor)
        //{
        //    var proxyFactory = Expression.Lambda<Func<ProxyContext, IInterceptor, T>>(
        //        Expression.New(proxyConstructor, _proxyDelegateParameterExpressions),
        //        _proxyDelegateParameterExpressions
        //    );

        //    return proxyFactory.Compile();
        //}

        private static Func<ProxyContext, IInterceptor, T> CreateProxyDelegate<T>(Type proxyType, ConstructorInfo proxyConstructor)
        {
            var dynamicMethod = new DynamicMethod(
                Constants.KimonoDelegateTypeNameFormat.ApplyFormat(proxyType.Name),
                typeof(T),
                _proxyDelegateParameters,
                proxyType
            );

            var iLGenerator = dynamicMethod.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Ldarg_1);
            iLGenerator.Emit(OpCodes.Newobj, proxyConstructor);
            iLGenerator.Emit(OpCodes.Ret);

            return (Func<ProxyContext, IInterceptor, T>)dynamicMethod.CreateDelegate(
                Constants.GetFuncProxyContextIInterceptorTType<T>()
            );
        }
    }
}