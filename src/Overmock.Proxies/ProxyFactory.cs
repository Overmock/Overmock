using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProxyFactory : IProxyFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interceptor"></param>
        /// <param name="argsProvider"></param>
        protected ProxyFactory(IInterceptor interceptor, IProxyCache cache)
        {
            Target = interceptor;
			Cache = cache;
		}

        /// <summary>
        /// 
        /// </summary>
        protected IInterceptor Target { get; }

        /// <summary>
        /// 
        /// </summary>
		protected IProxyCache Cache { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T Create<T>() where T : class
        {
            return (T)Create();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public object Create()
        {
            var context = CreateContext();
            return CreateCore(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IProxyBuilderContext CreateContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        protected abstract object CreateCore(IProxyBuilderContext marshallerContext);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static string GetName(IInterceptor interceptor) => Constants.AssemblyAndTypeNameFormat.ApplyFormat(interceptor.TypeName);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static AssemblyName GetAssemblyName(IInterceptor target) => new(Constants.AssemblyDllNameFormat.ApplyFormat(GetName(target)));

        /// <summary>
		/// 
		/// </summary>
		protected interface IProxyBuilderContext
        {
            /// <summary>
            /// 
            /// </summary>
			IInterceptor Target { get; }

            /// <summary>
            /// 
            /// </summary>
			TypeBuilder TypeBuilder { get; }

            /// <summary>
            /// 
            /// </summary>
            ProxyContext ProxyContext { get; }
        }
    }
}