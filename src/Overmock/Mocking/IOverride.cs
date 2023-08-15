
namespace Overmock.Mocking
{
    /// <summary>
    /// Represents a specific override of an overmock.
    /// </summary>
    public interface IOverride : IVerifiable
	{
		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		object? Handle(OvermockContext context);
	}
}