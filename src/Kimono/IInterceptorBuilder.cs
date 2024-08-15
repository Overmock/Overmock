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
        IInterceptorBuilder AddCallback(InterceptorBuilderAction action);

        /// <summary>
        /// Adds the specified action.
        /// </summary>
        /// <param name="handler">The action.</param>
        /// <returns>IInvocationChainBuilder.</returns>
        IInterceptorBuilder AddHandler(IInvocationHandler handler);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="disposable"></param>
        /// <returns></returns>
        IDisposableInterceptorBuilder<T> Dispose<T>(T disposable) where T : class, IDisposable;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <returns></returns>
        ITargetedInterceptorBuilder<T> Target<T>(T target) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IInterceptor.</returns>
        IInterceptor<T> Build<T>() where T : class;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ITargetedInterceptorBuilder<T> : IInterceptorBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDisposableInterceptorBuilder<T> DisposeResources();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>IInterceptor.</returns>
        IInterceptor<T> Build();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDisposableInterceptorBuilder<T> : IInterceptorBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>IInterceptor.</returns>
        IDisposableInterceptor<T> Build();
    }
}