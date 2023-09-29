namespace Kimono.Delegates
{
    /// <summary>
    /// Interface IMethodDelegateInvoker
    /// </summary>
    public interface IDelegateInvoker
    {
        /// <summary>
        /// Invokes the specified target.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="invocation"></param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;System.Object&gt;.</returns>
        object? Invoke(object? target, IInvocation invocation, params object?[] parameters);
    }
}