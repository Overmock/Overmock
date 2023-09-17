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
        protected void HandleDisposeCall()
        {
            (_interceptor as IDisposable)?.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected object? HandleMethodCall(int methodId, Type[] genericParameters, object[] parameters)
        {
            return Interceptor.HandleInvocation(methodId, genericParameters, parameters);
        }
    }
}