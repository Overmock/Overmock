
namespace Kimono
{
	/// <summary>
	/// Delegate InvocationAction
	/// </summary>
	/// <param name="context">The context.</param>
	public delegate void InvocationAction(IInvocationContext context);

	/// <summary>
	/// Delegate InvocationAction
	/// </summary>
	/// <param name="next"></param>
	/// <param name="context">The context.</param>
	public delegate void InvocationChainAction(InvocationAction next, IInvocationContext context);
}
