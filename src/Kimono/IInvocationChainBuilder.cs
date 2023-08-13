namespace Kimono
{
	/// <summary>
	/// Interface IInvocationChainBuilder
	/// </summary>
	public interface IInvocationChainBuilder : IFluentInterface
	{
		/// <summary>
		/// Adds the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>IInvocationChainBuilder.</returns>
		IInvocationChainBuilder Add(InvocationChainAction action);

		/// <summary>
		/// Adds the specified action.
		/// </summary>
		/// <param name="handler">The action.</param>
		/// <returns>IInvocationChainBuilder.</returns>
		IInvocationChainBuilder Add(IInvocationChainHandler handler);

		/// <summary>
		/// Builds this instance.
		/// </summary>
		/// <returns>IInvocationHandler.</returns>
		IInvocationHandler Build();
	}
}