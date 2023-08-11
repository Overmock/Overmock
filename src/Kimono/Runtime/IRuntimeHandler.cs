namespace Kimono
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
		/// <exception cref="KimonoException"></exception>
		public RuntimeHandlerResult Handle(IProxy proxy, params object[] parameters);
	}
}
