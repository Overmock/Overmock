using System.ComponentModel;

namespace Overmock.Verification.Internal
{
    public abstract class Verifiable : IVerifiable
    {
        protected Verifiable()
        {
        }

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

        internal readonly string TypeName = $"{_type.Name}_{Guid.NewGuid():N}";

        internal Verifiable() : base()
        {
        }

        protected ITypeBuilder Builder { get; } = Overmocked.Builder.GetTypeBuilder();

        Type IVerifiable<T>.Type => _type;

        protected override void Verify()
        {
            throw new NotImplementedException();
        }
    }
}