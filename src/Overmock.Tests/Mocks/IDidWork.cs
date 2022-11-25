namespace Overmock.Tests.Mocks
{
    public interface IDidWork<T>
    {
        T Result { get; }
    }
}