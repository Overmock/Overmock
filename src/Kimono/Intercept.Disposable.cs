using Kimono.Interceptors;
using Kimono.Interceptors.Internal;
using System;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// Contains methods for generating <see cref="IInterceptor"/>s.
    /// </summary>
    public static partial class Intercept
    {
        /// <summary>
        /// Intercepts the specified implementation.
        /// </summary>
        /// <typeparam name="T">The type of the t interface.</typeparam>
        /// <typeparam name="TTarget">The type of the t implementation.</typeparam>
        /// <param name="target">The implementation.</param>
        /// <param name="memberInvoked">The member invoked.</param>
        /// <returns>TypeInterceptor&lt;TInterface&gt;.</returns>
        public static T DisposableWithCallback<T, TTarget>(TTarget target, InvocationAction memberInvoked)
            where T : class, IDisposable
            where TTarget : T
        {
            return new DisposableTargetedCallbackInterceptor<T>(target, memberInvoked);
        }

        /// <summary>
        /// Intercepts member calls using the provided the handler.
        /// </summary>
        /// <typeparam name="T">The interface type to proxy.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
        /// <param name="target"></param>
        /// <param name="handler">The handler.</param>
        /// <returns>The interceptor.</returns>
        public static T DisposableWithHandlers<T, TTarget>(TTarget target, IInvocationHandler handler)
            where T : class, IDisposable
            where TTarget : T
        {
            return new DisposableTargetedSingleHandlerInterceptor<T>(target, handler);
        }

        /// <summary>
        /// Intercepts member calls using the provided the handlers.
        /// </summary>
        /// <typeparam name="T">The interface type to proxy.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
        /// <param name="target"></param>
        /// <param name="handlers">The handlers.</param>
        /// <returns>The interceptor.</returns>
        public static T DisposableWithHandlers<T, TTarget>(TTarget target, params IInvocationHandler[] handlers)
            where T : class, IDisposable
            where TTarget : T
        {
            return new DisposableTargetedHandlersInterceptor<T>(target, handlers);
        }

        /// <summary>
        /// Intercepts member calls using the provided the handlers.
        /// </summary>
        /// <typeparam name="T">The interface type to proxy.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy to.</typeparam>
        /// <param name="target"></param>
        /// <param name="handlers">The handlers.</param>
        /// <returns>The interceptor.</returns>
        public static T DisposableWithHandlers<T, TTarget>(TTarget target, IEnumerable<IInvocationHandler> handlers)
            where T : class, IDisposable
            where TTarget : T
        {
            return new DisposableTargetedHandlersInterceptor<T>(target, handlers);
        }

        /// <summary>
        /// Withes the inovcation chain.
        /// </summary>
        /// <typeparam name="T">The type of the t interface.</typeparam>
        /// <typeparam name="TTarget">The target type to proxy call to.</typeparam>
        /// <param name="target"></param>
        /// <param name="builderAction">The builder action.</param>
        /// <returns>TInterface.</returns>
        public static T DisposableWithInovcationChain<T, TTarget>(TTarget target, Action<IInvocationChainBuilder> builderAction)
            where T : class, IDisposable
            where TTarget : T
        {
            var builder = new InvocationChainBuilder();
            builderAction(builder);
            return new DisposableTargetedSingleHandlerInterceptor<T>(target, builder.Build());
        }
    }
}
