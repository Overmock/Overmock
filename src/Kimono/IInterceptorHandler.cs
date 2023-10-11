namespace Kimono
{
    /// <summary>
    /// Interface IInterceptorHandler
    /// </summary>
    public interface IInterceptorHandler
    {
        /// <summary>
        /// Handles the specified next.
        /// </summary>
        /// <param name="nextHandler">The next action to call in the chain.</param>
        /// <param name="invocation">The context.</param>
        void Handle(InvocationAction nextHandler, IInvocation invocation);
    }
}