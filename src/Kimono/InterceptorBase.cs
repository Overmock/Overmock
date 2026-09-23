namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class InterceptorBase<T> : Interceptor<T> where T : class
    {
        /// <summary>
        /// Initializes an interceptor with no target. Invocations are handled by
        /// <see cref="HandleInvocationCore"/> only; there is nothing to fall through to.
        /// </summary>
        protected InterceptorBase()
        {
        }

        /// <summary>
        /// Initializes an interceptor wrapping <paramref name="target"/>. Without this there was no
        /// way to give a target to the one base class whose whole purpose is to invoke the target
        /// when the handler did not.
        /// </summary>
        /// <param name="target"></param>
        protected InterceptorBase(T? target) : base(target)
        {
        }

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