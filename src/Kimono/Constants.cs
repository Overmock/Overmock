using System.Globalization;
using System.Reflection;
using Kimono.Proxies;

namespace Kimono
{
    /// <summary>
    /// Class Constants.
    /// </summary>
    internal static class Constants
    {
		/// <summary>
		/// Gets the current culture.
		/// </summary>
		/// <value>The current culture.</value>
		public static CultureInfo CurrentCulture => CultureInfo.CurrentCulture;

		/// <summary>
		/// The assembly identifier
		/// </summary>
		public static readonly string AssemblyId = Guid.NewGuid().ToString();

		/// <summary>
		/// The assembly DLL name format
		/// </summary>
		public const string AssemblyDllNameFormat = "{0}.dll";

		/// <summary>
		/// The assembly and type name format
		/// </summary>
		public const string AssemblyAndTypeNameFormat = "Kimono-DyamicProxies.{0}";

        /// <summary>
        /// The kimono deleate type name format
        /// </summary>
        public static readonly string KimonoDelegateTypeNameFormat = "Kimono_Proxy_Factory_{0}";

        /// <summary>
        /// The invoke method name
        /// </summary>
        public const string InvokeMethodName = "Invoke";

        /// <summary>
        /// The void type
        /// </summary>
        public static readonly Type VoidType = typeof(void);

		/// <summary>
		/// The object type
		/// </summary>
		public static readonly Type ObjectType = typeof(Object);

		/// <summary>
		/// The object array type
		/// </summary>
		public static readonly Type ObjectArrayType = typeof(Object[]);

		/// <summary>
		/// The delegate type
		/// </summary>
		public static readonly Type DelegateType = typeof(Delegate);

		/// <summary>
		/// The proxy type
		/// </summary>
		public static readonly Type ProxyType = typeof(ProxyBase<>);

		/// <summary>
		/// The method base type
		/// </summary>
		public static readonly Type MethodBaseType = typeof(MethodBase);

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
		/// The kimono context type
		/// </summary>
		public static readonly Type ProxyContextType = typeof(ProxyContext);

        /// <summary>
        /// The kimono context type
        /// </summary>
        public static readonly Type IInterceptorType = typeof(IInterceptor);

        /// <summary>
        /// The disposable type
        /// </summary>
        public static readonly Type DisposableType = typeof(IDisposable);

        /// <summary>
        /// The <see cref="Func{ProxyContext, IInterceptor, T}"/> type
        /// </summary>
        public static Type GetFuncProxyContextIInterceptorTType<T>() => typeof(Func<ProxyContext, IInterceptor, T>);

        /// <summary>
        /// The disposable method.
        /// </summary>
        public static readonly MethodInfo DisposeMethod = DisposableType.GetMethod(nameof(IDisposable.Dispose))!;

		/// <summary>
		/// The empty object array method
		/// </summary>
		public static readonly MethodInfo EmptyObjectArrayMethod = EmptyArrayMethod(ObjectType);

		/// <summary>
		/// The method base type get current method
		/// </summary>
		public static readonly MethodInfo MethodBaseTypeGetCurrentMethod = MethodBaseType.GetMethod("GetCurrentMethod", BindingFlags.Static | BindingFlags.Public)!;
        
        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action1ObjectType = typeof(Action<object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action2ObjectType = typeof(Action<object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action3ObjectType = typeof(Action<object?, object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action4ObjectType = typeof(Action<object?, object?, object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action5ObjectType = typeof(Action<object?, object?, object?, object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Action6ObjectType = typeof(Action<object?, object?, object?, object?, object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func1ObjectType = typeof(Func<object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func2ObjectType = typeof(Func<object?, object?, object?>);

        /// <summary>
        /// The action type taking one object.
        /// </summary>
        public static readonly Type Func3ObjectType = typeof(Func<object?, object?, object?, object?>);

        /// <summary>
        /// The func type taking 3 objects.
        /// </summary>
        public static readonly Type Func4ObjectType = typeof(Func<object?, object?, object?, object?, object?>);

        /// <summary>
        /// The func type taking 5 objects.
        /// </summary>
        public static readonly Type Func5ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?>);

        /// <summary>
        /// The func type taking 6 objects.
        /// </summary>
        public static readonly Type Func6ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?, object?>);

        /// <summary>
        /// Gets the proxy type handle method call method.
        /// </summary>
        /// <returns>MethodInfo.</returns>
        public static MethodInfo ProxyTypeHandleMethodCallMethod = ProxyType.GetMethod("HandleMethodCall", BindingFlags.Instance | BindingFlags.NonPublic)!;

		/// <summary>
		/// Gets the proxy type handle method call method.
		/// </summary>
		/// <returns>MethodInfo.</returns>
		public static MethodInfo ProxyTypeHandleDisposeCallMethod = ProxyType.GetMethod("HandleDisposeCall", BindingFlags.Instance | BindingFlags.NonPublic)!;

		/// <summary>
		/// Empties the array method.
		/// </summary>
		/// <param name="genericTypeArg">The generic type argument.</param>
		/// <returns>MethodInfo.</returns>
		public static MethodInfo EmptyArrayMethod(Type genericTypeArg) => ArrayType.GetMethod("Empty", BindingFlags.Static | BindingFlags.Public)!.MakeGenericMethod(genericTypeArg)!;

		/// <summary>
		/// Formats the specified format provider.
		/// </summary>
		/// <param name="formatProvider">The format provider.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>System.String.</returns>
		public static string Format(Func<string> formatProvider, params object[] args)
        {
            return string.Format(CurrentCulture, formatProvider(), args);
        }
	}
}