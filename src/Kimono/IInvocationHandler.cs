namespace Kimono
{
	/// <summary>
	/// An interface to handle member invocations
	/// </summary>
	public interface IInvocationHandler
	{
		/// <summary>
		/// Handles member invocations.
		/// </summary>
		/// <param name="context">The context.</param>
		void Handle(IInvocationContext context);
	}
}