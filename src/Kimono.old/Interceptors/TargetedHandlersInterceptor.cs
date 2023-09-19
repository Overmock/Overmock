using System.Collections.Generic;

namespace Kimono.Interceptors
{
    /// <summary>
    /// Class HandlersInterceptor.
    /// Implements the <see cref="Interceptor{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Interceptor{T}" />
    public class TargetedHandlersInterceptor<T> : HandlersInterceptor<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandlersInterceptor{T}" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="handlers">The handlers.</param>
        public TargetedHandlersInterceptor(T target, IEnumerable<IInvocationHandler> handlers) : base(handlers)
        {
            Target = target;
        }
    }
}
