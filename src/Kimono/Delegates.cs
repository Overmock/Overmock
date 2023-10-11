
namespace Kimono
{
    /// <summary>
    /// Delegate InvocationAction
    /// </summary>
    /// <param name="invocation">The context.</param>
    public delegate void InvocationAction(IInvocation invocation);

    /// <summary>
    /// Delegate InvocationAction
    /// </summary>
    /// <param name="next"></param>
    /// <param name="invocation">The context.</param>
    public delegate void InterceptorAction(InvocationAction next, IInvocation invocation);
}
