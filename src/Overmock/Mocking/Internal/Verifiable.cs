using System.ComponentModel;

namespace Overmock.Mocking.Internal
{
    public abstract class Verifiable : IVerifiable
    {
        internal readonly string TypeName;

        protected Verifiable(Type type)
        {
            // Look into how this name is generated for reading types back from disk.
            TypeName = $"{type.Name}_{Guid.NewGuid():N}";
            Type = type;
        }

        public Type Type { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void IVerifiable.Verify()
        {
            Verify();
        }

        protected abstract void Verify();
    }

    public abstract class Verifiable<T> : Verifiable, IVerifiable<T>
    {
        private static readonly Type _type = typeof(T);

        internal Verifiable() : base(typeof(T))
        {
        }
    }
}