namespace Kimono
{
	/// <summary>
	/// Interface IInvocationChainBuilder
	/// </summary>
	public interface IInvocationChainBuilder
	{
		/// <summary>
		/// Adds the specified action.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <returns>IInvocationChainBuilder.</returns>
		IInvocationChainBuilder Add(InvocationChainAction action);

		/// <summary>
		/// Builds this instance.
		/// </summary>
		/// <returns>IInvocationHandler.</returns>
		IInvocationHandler Build();
	}
}