using Overmock.Proxies.Internal;
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
        protected ProxyFactory(IProxyCache cache)
        {
			Cache = cache;
		}

        /// <summary>
        /// 
        /// </summary>
		protected IProxyCache Cache { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public IProxyGenerator<T> Create<T>(IInterceptor<T> interceptor) where T : class
		{
			var generator = (IProxyGenerator<T>)Cache.Get(interceptor.TargetType)!;

			if (generator == null)
			{
				var context = CreateContext(interceptor);
				generator = CreateCore<T>(context);
			}

			return generator;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IProxyBuilderContext CreateContext(IInterceptor interceptor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        protected abstract IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext marshallerContext) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static string GetName(string name) => Constants.AssemblyAndTypeNameFormat.ApplyFormat(name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected static AssemblyName GetAssemblyName(string name) => new(Constants.AssemblyDllNameFormat.ApplyFormat(GetName(name)));

        /// <summary>
		/// 
		/// </summary>
		protected interface IProxyBuilderContext
        {
            /// <summary>
            /// 
            /// </summary>
			IInterceptor Interceptor { get; }

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