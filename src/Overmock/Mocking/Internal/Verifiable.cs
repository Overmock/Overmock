using System.ComponentModel;

namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Verifiable : IVerifiable
    {
        internal readonly string _typeName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Verifiable"/> class.
        /// </summary>
        /// <param name="type"></param>
        protected Verifiable(Type type)
        {
            // Look into how this name is generated for reading types back from disk.
            _typeName = $"{type.Name}_{Guid.NewGuid():N}";
            Type = type;
        }

        /// <summary>
        /// 
        /// </summary>
        public Type Type { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IVerifiable.Verify()
        {
            Verify();
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void Verify();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Verifiable<T> : Verifiable, IVerifiable<T>
    {
        private static readonly Type _type = typeof(T);

        internal Verifiable() : base(typeof(T))
        {
        }
    }
}