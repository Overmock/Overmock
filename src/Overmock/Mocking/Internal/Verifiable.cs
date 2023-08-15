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
		/// Gets the type.
		/// </summary>
		/// <value>The type.</value>
		public Type Type { get; protected set; }

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
        internal readonly string _typeName;

        /// <summary>
        /// The type
        /// </summary>
        private static readonly Type _type = typeof(T);

		/// <summary>
		/// Initializes a new instance of the <see cref="Verifiable{T}"/> class.
		/// </summary>
		internal Verifiable() : base()
        {
            _typeName = $"{_type.Name}_{Guid.NewGuid():N}";

            Type = _type;
        }
	}
}