
namespace Kimono.Interceptors
{
    /// <summary>
    /// Class SingleHandlerInterceptor.
    /// Implements the <see cref="Kimono.Interceptor{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Kimono.Interceptor{T}" />
    public class HandlerInterceptor<T> : Interceptor<T>, IInvocationHandlerProvider where T : class
    {
        private readonly IInvocationHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlerInterceptor{T}"/> class.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public HandlerInterceptor(IInvocationHandler handler)
        {
            _handler = handler;
        }

        /// <inheritdoc />
        protected override void MemberInvoked(IInvocationContext context)
        {
            _handler.Handle(context);
        }

        IInvocationHandler IInvocationHandlerProvider.GetHandler() => _handler;
    }
}
