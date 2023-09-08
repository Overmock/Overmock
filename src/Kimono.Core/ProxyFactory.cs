using Kimono.Core.Internal;
using Microsoft.VisualBasic;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace Kimono.Core
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ProxyFactory : IProxyFactory
    {
        static ProxyFactory()
        {
            var assemblyName = new AssemblyName(Names.DllName);

            CurrentAssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
                assemblyName,
                AssemblyBuilderAccess.Run);

            ModuleBuilder = CurrentAssemblyBuilder.DefineDynamicModule(Names.ModuleName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delegateFactory"></param>
        public ProxyFactory(IDelegateFactory delegateFactory, IProxyCache cache)
        {
            DelegateFactory = delegateFactory;
            Cache = cache;
        }

        private static AssemblyBuilder CurrentAssemblyBuilder { get; }

        private static ModuleBuilder ModuleBuilder { get; }

        public IDelegateFactory DelegateFactory { get; }

        public IProxyCache Cache { get; }

        public static IProxyFactory Create()
        {
            return new ProxyFactory(new DelegateFactory(), ProxyCache.Instance);
        }

        public T CreateInterfaceProxy<T>() where T : class
        {
            const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

            var proxyType = Types.ProxyBase.MakeGenericType(typeof(T));

            var builder = ModuleBuilder.DefineType(string.Format(Names.TypeName, typeof(T).Name), TypeAttributes.Public | TypeAttributes.Sealed, Types.ProxyBase);

            //ctor
            var constructor = proxyType.GetConstructor(
                bindingFlags, null,
                Types.ProxyBaseCtorParameterTypes<T>(), null
            )!;


            //methods


            //properties



            return default;
        }

        private static class Names
        {
            public const string DllName = "KimonoProxies.{0}.dll";
            public const string Namesapce = "KimonoProxies.{0}";
            public const string ModuleName = "KimonoProxies";
            public const string TypeName = "Proxy-{0}";
        }

        private static class Types
        {
            /// <summary>
            /// The method information type
            /// </summary>
            public static readonly Type MethodInfoType = typeof(MethodInfo);

            /// <summary>
            /// The array type
            /// </summary>
            public static readonly Type ArrayType = typeof(Array);

            /// <summary>
            /// The type type
            /// </summary>
            public static readonly Type TypeType = typeof(Type);

            /// <summary>
            /// The array type
            /// </summary>
            public static readonly Type TypeArrayType = typeof(Type[]);

            public static readonly Type ProxyBase = typeof(ProxyBase<>);

            /// <summary>
            /// The kimono context type
            /// </summary>
            public static Type IInterceptorType<T>() => typeof(IInterceptor<T>);

            /// <summary>
            /// The proxy type
            /// </summary>
            public static Type[] ProxyBaseCtorParameterTypes<T>() => new Type[]
            {
                IInterceptorType<T>()
            };
        }
    }
}
