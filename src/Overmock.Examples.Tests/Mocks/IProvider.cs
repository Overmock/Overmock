
namespace Overmock.Tests.Mocks
{
    public interface IProvider
    {
        string GetName();

        IDictionary<string, object> GetProperties();
    }
}