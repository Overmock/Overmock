namespace Overmock.Runtime
{
    public static class OvermockExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsInterface(this IOvermock target)
        {
            return target.Type.IsInterface();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsDelegate(this IOvermock target)
        {
            return target.Type.IsDelegate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsInterface(this Type target)
        {
            return target is { IsInterface: true };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool IsDelegate(this Type target)
        {
            return Constants.DelegateType.IsAssignableFrom(target);
        }
    }
}