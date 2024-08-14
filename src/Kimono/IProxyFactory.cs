using System;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProxyFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="callback"></param>
        /// <returns></returns>
        T CreateInterfaceProxy<T>(Action<IInvocation> callback) where T : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interceptor"></param>
        /// <returns></returns>
        T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class;

        /// <summary>
        /// Creates a <see cref="IProxyGenerator{T}"/> for the specified interceptor. Note: this ignores any cached generators.
        /// </summary>
        /// <param name="interceptor"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="KimonoException"></exception>
        IProxyGenerator<T> CreateProxyGenerator<T>(IInterceptor<T> interceptor) where T : class;
    }
}
