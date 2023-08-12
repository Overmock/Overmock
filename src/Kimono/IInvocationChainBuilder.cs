namespace Kimono
{
	/// <summary>
	/// Interface IInvocationChainBuilder
	/// </summary>
	public interface IInvocationChainBuilder
	{
		/// <summary>
		/// Builds this instance.
		/// </summary>
		/// <returns>IInvocationHandler.</returns>
		IInvocationHandler Build();
	}
}