﻿using System.Reflection;
using System.Reflection.Emit;

namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ProxyMarshaller : IMarshaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interceptor"></param>
        /// <param name="argsProvider"></param>
        protected ProxyMarshaller(IInterceptor interceptor)
        {
            Target = interceptor;
        }

        /// <summary>
        /// 
        /// </summary>
        protected IInterceptor Target { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Marshal<T>() where T : class
        {
            return (T)Marshal();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public object Marshal()
        {
            var context = CreateContext();
            return MarshalCore(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract IMarshallerContext CreateContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="marshallerContext"></param>
        /// <returns></returns>
        protected abstract object MarshalCore(IMarshallerContext marshallerContext);

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
		protected interface IMarshallerContext
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