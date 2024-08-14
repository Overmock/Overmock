namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class InterceptorBase<T> : Interceptor<T> where T : class
    {
        /// <inheritdoc />
        protected override void HandleInvocation(IInvocation invocation)
        {
            HandleInvocationCore(invocation);

            if (!invocation.TargetInvoked)
            {
                base.HandleInvocation(invocation);
            }
        }

        /// <summary>
        /// Called when an invocation is intercepted.
        /// </summary>
        /// <param name="invocation"></param>
        protected abstract void HandleInvocationCore(IInvocation invocation);
    }
}