using System.ComponentModel;

namespace Overmock.Mocking
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IVerifiable : IFluentInterface
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        Type Type { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        void Verify();
    }

    public interface IVerifiable<T> : IVerifiable
    {
    }
}