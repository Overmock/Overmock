namespace Overmock.Mocking
{
	/// <summary>
	/// Represents a member that is overridden
	/// </summary>
	public interface ICallable : IThrowable
	{
		/// <summary>
		/// Gets or sets the times.
		/// </summary>
		/// <value>The times.</value>
		Times Times { get; set; }

		/// <summary>
		/// An <see cref="Func{OverrideContext, TReturn}" /> delegate to call in place of this override's property.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="times">The times.</param>
		void Calls(Action<OvermockContext> action, Times times);


		/// <summary>
		/// Gets the default return value.
		/// </summary>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		object? GetDefaultReturnValue();
	}

	/// <summary>
	/// Interface ICallable
	/// Extends the <see cref="Overmock.Mocking.ICallable" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.Mocking.ICallable" />
	public interface ICallable<T> : ICallable
	{
	}
}