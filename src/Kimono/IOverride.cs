
namespace Kimono
{
	/// <summary>
	/// Represents a specific override of an Kimono.
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