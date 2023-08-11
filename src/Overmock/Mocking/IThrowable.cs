namespace Overmock.Mocking
{
	/// <summary>
	/// Interface IThrowable
	/// Extends the <see cref="Overmock.Mocking.IOverridable" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverridable" />
	public interface IThrowable : IOverridable
	{
		/// <summary>
		/// Throws the exception when called.
		/// </summary>
		/// <param name="exception">The exception.</param>
		void Throws(Exception exception);
	}
}