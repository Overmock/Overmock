namespace Kimono
{
	/// <summary>
	/// Represents a specific override of an Kimono.
	/// </summary>
	public interface IOverride
	{
		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		object? Handle(RuntimeContext context);
	}
}