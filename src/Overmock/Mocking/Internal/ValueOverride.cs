
namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class ValueOverride.
	/// Implements the <see cref="Overmock.Mocking.IOverride" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverride" />
	internal class ValueOverride : IOverride
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ValueOverride"/> class.
		/// </summary>
		/// <param name="value">The value.</param>
		public ValueOverride(object value)
		{
			Value = value;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <value>The value.</value>
		public object Value { get; }

		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public object? Handle(OvermockContext context)
		{
			return Value;
		}
	}
}