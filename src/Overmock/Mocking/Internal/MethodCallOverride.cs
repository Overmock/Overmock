
namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class MethodCallOverride.
	/// Implements the <see cref="Overmock.Mocking.IOverride" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverride" />
	internal class MethodCallOverride : IOverride
	{
		/// <summary>
		/// The call count
		/// </summary>
		private int _callCount;
		/// <summary>
		/// Initializes a new instance of the <see cref="MethodCallOverride"/> class.
		/// </summary>
		/// <param name="overmock">The overmock.</param>
		/// <param name="times">The times.</param>
		public MethodCallOverride(Delegate overmock, Times times)
		{
			Overmock = overmock;
			Times = times;
		}

		/// <summary>
		/// Gets the overmock.
		/// </summary>
		/// <value>The overmock.</value>
		public Delegate Overmock { get; }

		/// <summary>
		/// Gets the times.
		/// </summary>
		/// <value>The times.</value>
		public Times Times { get; }

		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public object? Handle(OvermockContext context)
		{
			Times.ThrowIfInvalid(++_callCount);
			return Overmock.DynamicInvoke(context);
		}
	}
}