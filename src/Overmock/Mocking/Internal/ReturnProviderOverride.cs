
namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class ReturnProviderOverride.
	/// Implements the <see cref="Overmock.Mocking.IOverride" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverride" />
	public class ReturnProviderOverride : IOverride
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReturnProviderOverride"/> class.
		/// </summary>
		/// <param name="returnProvider">The return provider.</param>
		internal ReturnProviderOverride(Func<object> returnProvider)
		{
			ReturnProvider = returnProvider;
		}

		/// <summary>
		/// Gets the return provider.
		/// </summary>
		/// <value>The return provider.</value>
		public Func<object> ReturnProvider { get; }

		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public object? Handle(OvermockContext context)
		{
			return ReturnProvider();
		}
	}
}