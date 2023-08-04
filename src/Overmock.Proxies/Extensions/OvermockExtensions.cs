namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public static class OvermockExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool IsInterface(this IInterceptor target)
        {
            return target.TargetType.IsInterface();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool IsDelegate(this IInterceptor target)
        {
            return target.TargetType.IsDelegate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool IsInterface(this Type target)
        {
            return target is { IsInterface: true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        internal static bool IsDelegate(this Type target)
        {
            return Constants.DelegateType.IsAssignableFrom(target);
		}

		internal static string ApplyFormat(this string format, params object[] args)
		{
			return Constants.Format(() => format, args);
		}
	}
}