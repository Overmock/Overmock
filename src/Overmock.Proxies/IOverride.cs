
namespace Overmock.Proxies
{
	/// <summary>
	/// Represents a specific override of an overmock.
	/// </summary>
	public interface IOverride
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		object? Handle(RuntimeContext context);
	}
}