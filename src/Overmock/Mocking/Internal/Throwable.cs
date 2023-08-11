namespace Overmock.Mocking.Internal
{

	/// <summary>
	/// Class Throwable.
	/// Implements the <see cref="Overmock.Mocking.Internal.Overridable" />
	/// Implements the <see cref="Overmock.Mocking.IThrowable" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.Internal.Overridable" />
	/// <seealso cref="Overmock.Mocking.IThrowable" />
	internal abstract class Throwable : Overridable, IThrowable
	{
		/// <summary>
		/// Gets the exception.
		/// </summary>
		/// <value>The exception.</value>
		public Exception? Exception { get; private set; }

		/// <inheritdoc />
		public void Throws(Exception exception)
		{
			Exception = exception;
		}

		/// <inheritdoc />
		protected override void AddOverridesTo(List<IOverride> overrides)
		{
			if (Exception != null)
			{
				overrides.Add(new ThrowExceptionOverride(Exception));
			}
		}
	}
}