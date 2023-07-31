namespace Overmock.Proxies
{
	/// <summary>
	/// Handles a specific <see cref="RuntimeContext" />.
	/// </summary>
	public interface IRuntimeHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="OvermockException"></exception>
		public RuntimeHandlerResult Handle(params object[] parameters);
	}
}
