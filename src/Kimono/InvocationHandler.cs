namespace Kimono
{
    /// <summary>
    /// When inherited, provides a base implementation for an invocation handler.
    /// </summary>
    public abstract class InvocationHandler : IInvocationHandler
    {
        /// <inheritdoc />
        public void Handle(InvocationAction nextHandler, IInvocation invocation)
        {
            //TODO: Initialize or update any state here before calling the handler.

            // Call the derived class to handle the invocation
            HandleCore(nextHandler, invocation);
        }

        /// <summary>
        /// When overridden in a derived class, is called to handle the invocation.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="invocation"></param>
        protected abstract void HandleCore(InvocationAction next, IInvocation invocation);
    }
}