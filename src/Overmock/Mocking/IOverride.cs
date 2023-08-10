
namespace Overmock.Mocking
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
		object? Handle(OvermockContext context);
	}
}