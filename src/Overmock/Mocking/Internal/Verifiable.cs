using System.ComponentModel;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class Verifiable.
	/// Implements the <see cref="Overmock.Mocking.IVerifiable" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IVerifiable" />
	public abstract class Verifiable : IVerifiable
	{
		/// <summary>
		/// The type name
		/// </summary>
		internal readonly string _typeName;

		/// <summary>
		/// Initializes a new instance of the <see cref="Verifiable" /> class.
		/// </summary>
		/// <param name="type">The type.</param>
		protected Verifiable(Type type)
		{
			// Look into how this name is generated for reading types back from disk.
			_typeName = $"{type.Name}_{Guid.NewGuid():N}";
			Type = type;
		}

		/// <summary>
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public Type Type { get; }

		/// <summary>
		/// Verifies this instance.
		/// </summary>
		[EditorBrowsable(EditorBrowsableState.Never)]
		void IVerifiable.Verify()
		{
			Verify();
		}

		/// <summary>
		/// Verifies this instance.
		/// </summary>
		protected abstract void Verify();
	}

	/// <summary>
	/// Class Verifiable.
	/// Implements the <see cref="Overmock.Mocking.Internal.Verifiable" />
	/// Implements the <see cref="Overmock.Mocking.IVerifiable{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.Mocking.Internal.Verifiable" />
	/// <seealso cref="Overmock.Mocking.IVerifiable{T}" />
	public abstract class Verifiable<T> : Verifiable, IVerifiable<T>
	{
		/// <summary>
		/// The type
		/// </summary>
		private static readonly Type _type = typeof(T);

		/// <summary>
		/// Initializes a new instance of the <see cref="Verifiable{T}"/> class.
		/// </summary>
		internal Verifiable() : base(typeof(T))
		{
		}
	}
}