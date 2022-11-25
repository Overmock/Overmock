using System.ComponentModel;

namespace Overmock.Verification
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IVerifiable : IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        void Verify();
    }

    public interface IVerifiable<T> : IVerifiable
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type Type { get; }
    }
}