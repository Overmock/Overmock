using System;

namespace Kimono
{
    /// <summary>
    /// Interface IInvocationChainBuilder
    /// </summary>
    public interface IInterceptorBuilder : IFluentInterface
    {
        /// <summary>
        /// Adds the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>IInvocationChainBuilder.</returns>
        IInterceptorBuilder Add(InterceptorAction action);

        /// <summary>
        /// Adds the specified action.
        /// </summary>
        /// <param name="handler">The action.</param>
        /// <returns>IInvocationChainBuilder.</returns>
        IInterceptorBuilder Add(IInterceptorHandler handler);

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IInterceptor.</returns>
        IInterceptor<T> Build<T>() where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable"></param>
        /// <returns></returns>
        IDisposableInterceptor<T> Build<T>(T disposable) where T : class, IDisposable;
    }
}