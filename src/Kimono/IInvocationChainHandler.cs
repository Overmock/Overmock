namespace Kimono
{
	/// <summary>
	/// Interface IInvocationChainHandler
	/// </summary>
	public interface IInvocationChainHandler
	{
		/// <summary>
		/// Handles the specified next.
		/// </summary>
		/// <param name="nextHandler">The next action to call in the chain.</param>
		/// <param name="context">The context.</param>
		void Handle(InvocationAction nextHandler, IInvocationContext context);
	}
}