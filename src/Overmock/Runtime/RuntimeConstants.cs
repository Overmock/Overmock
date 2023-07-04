using Overmock.Runtime.Proxies;

namespace Overmock.Runtime
{
    internal static class RuntimeConstants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string AssemblyDllNameFormat = "{0}.dll";

        /// <summary>
        /// 
        /// </summary>
        public const string AssemblyNameFormat = "Overmocked.Proxies.{0}";

        /// <summary>
        /// 
        /// </summary>
        public const string InvokeMethodName = "Invoke";

        /// <summary>
        /// 
        /// </summary>
        public static readonly Type DelegateType = typeof(Delegate);

        /// <summary>
        /// 
        /// </summary>
        public static readonly Type ProxyType = typeof(Proxy<>);
    }
}