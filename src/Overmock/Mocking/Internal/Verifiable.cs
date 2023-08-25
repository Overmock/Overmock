using System;
using System.ComponentModel;

namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// Class Verifiable.
    /// Implements the <see cref="IVerifiable" />
    /// </summary>
    /// <seealso cref="IVerifiable" />
    public abstract class Verifiable : IVerifiable
    {
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
    /// Implements the <see cref="Verifiable" />
    /// Implements the <see cref="IVerifiable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Verifiable" />
    /// <seealso cref="IVerifiable{T}" />
    public abstract class Verifiable<T> : Verifiable, IVerifiable<T>
    {
        internal readonly string _typeName;

        /// <summary>
        /// The type
        /// </summary>
        protected static readonly Type Type = typeof(T);

        /// <summary>
        /// Initializes a new instance of the <see cref="Verifiable{T}"/> class.
        /// </summary>
        internal Verifiable()
        {
            _typeName = $"{Type.Name}_{Guid.NewGuid():N}";
        }
    }
}