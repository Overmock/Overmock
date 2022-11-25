
namespace Overmock.Tests.Mocks
{
    public interface IProvider
    {
        string GetName();

        IDisposable GetProperties();
    }
}