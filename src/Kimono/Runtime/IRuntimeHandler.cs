namespace Kimono
{
	/// <summary>
	/// Handles a specific <see cref="RuntimeContext" />.
	/// </summary>
	public interface IRuntimeHandler
	{
		/// <summary>
		/// Handles the specified proxy.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>RuntimeHandlerResult.</returns>
		/// <exception cref="KimonoException"></exception>
		public RuntimeHandlerResult Handle(IProxy proxy, params object[] parameters);
	}
}
