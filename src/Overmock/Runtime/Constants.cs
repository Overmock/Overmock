using Overmock.Runtime.Proxies;
using System.Globalization;
using System.Reflection;

namespace Overmock.Runtime
{
    internal static class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public static CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

        /// <summary>
        /// 
        /// </summary>
        public const string AssemblyDllNameFormat = "{0}.dll";

        /// <summary>
        /// 
        /// </summary>
        public const string AssemblyAndTypeNameFormat = "Overmocked.Proxies.{0}";

        /// <summary>
        /// 
        /// </summary>
        public const string InvokeMethodName = "Invoke";

		/// <summary>
		/// 
		/// </summary>
		public const string InitializeOvermockContextMethodName = nameof(IProxy.InitializeOvermockContext);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Type OvermockType = typeof(IOvermock);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Type ObjectType = typeof(Object);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Type ObjectArrayType = typeof(Object[]);

		/// <summary>
		/// 
		/// </summary>
		public static readonly Type DelegateType = typeof(Delegate);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Type ProxyType = typeof(Proxy<>);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type MethodBaseType = typeof(MethodBase);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type MethodInfoType = typeof(MethodInfo);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type OverrideHandlerType = typeof(IRuntimeHandler);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type ArrayType = typeof(Array);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type TypeType = typeof(Type);

        /// <summary>
        /// 
        /// </summary>
		public static readonly Type OvermockContextType = typeof(ProxyOverrideContext);

        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo EmptyObjectArrayMethod = EmptyArrayMethod(ObjectType);

        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo GetTypeFromHandleMethod = TypeType.GetMethod("GetTypeFromHandle", BindingFlags.Static | BindingFlags.Public)!;

		/// <summary>
		/// 
		/// </summary>
		public static readonly MethodInfo OverrideHandlerTypeHandleMethod = OverrideHandlerType.GetMethod("Handle", BindingFlags.Instance | BindingFlags.Public)!;

        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo MethodBaseTypeGetCurrentMethod = MethodBaseType.GetMethod("GetCurrentMethod", BindingFlags.Static | BindingFlags.Public)!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericTypeArg"></param>
        /// <returns></returns>
        public static MethodInfo EmptyArrayMethod(Type genericTypeArg) => ArrayType.GetMethod("Empty", BindingFlags.Static | BindingFlags.Public)!.MakeGenericMethod(genericTypeArg)!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericTypeArg"></param>
        /// <returns></returns>
        public static MethodInfo GetProxyTypeHandleMethodCallMethod(Type genericTypeArg) =>
            ProxyType.MakeGenericType(genericTypeArg).GetMethod("HandleMethodCall", BindingFlags.Instance | BindingFlags.Public)!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Format(Func<string> formatProvider, params object[] args)
        {
            return string.Format(CurrentCulture, formatProvider(), args);
        }
	}
}