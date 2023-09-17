using System;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public abstract class ProxyBase : IProxy
    {
        private readonly IInterceptor _interceptor;

        /// <summary>
        /// 
        /// </summary>
        protected ProxyBase(IInterceptor interceptor)
        {
            _interceptor = interceptor;
        }

        /// <summary>
        /// 
        /// </summary>
        public IInterceptor Interceptor => _interceptor;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected abstract object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters);

        /// <summary>
        /// 
        /// </summary>
        protected void HandleDisposeCall()
        {
            (_interceptor as IDisposable)?.Dispose();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ProxyBase<T> : ProxyBase, IProxy<T>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interceptor"></param>
        protected ProxyBase(IInterceptor interceptor) : base(interceptor)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected sealed override object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters)
        {
            return Interceptor.HandleInvocation(methodId, genericParameters, parameters);
        }
    }
}