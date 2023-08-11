using System.ComponentModel;

namespace Overmock.Mocking
{
	/// <summary>
	/// Interface IVerifiable
	/// Extends the <see cref="Overmock.IFluentInterface" />
	/// </summary>
	/// <seealso cref="Overmock.IFluentInterface" />
	[EditorBrowsable(EditorBrowsableState.Never)]
	public interface IVerifiable : IFluentInterface
	{
		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		[EditorBrowsable(EditorBrowsableState.Never)]
		Type Type { get; }

		/// <summary>
		/// Verifies this instance.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		void Verify();
	}

	/// <summary>
	/// Interface IVerifiable
	/// Extends the <see cref="Overmock.Mocking.IVerifiable" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.Mocking.IVerifiable" />
	public interface IVerifiable<T> : IVerifiable
	{
	}
}