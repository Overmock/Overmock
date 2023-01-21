namespace Overmock.Runtime
{
	/// <summary>
	/// Handles a specific <see cref="OverrideContext" />.
	/// </summary>
	public interface IOverrideHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		/// <exception cref="OvermockException"></exception>
		public OverrideHandlerResult Handle(params object[] parameters);
	}
}
