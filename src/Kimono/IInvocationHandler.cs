namespace Kimono
{
    /// <summary>
    /// Interface IInterceptorHandler
    /// </summary>
    public interface IInvocationHandler
    {
        /// <summary>
        /// Handles the specified next.
        /// </summary>
        /// <param name="next">The next action to call in the chain.</param>
        /// <param name="invocation">The context.</param>
        void Handle(InvocationAction next, IInvocation invocation);
    }
}