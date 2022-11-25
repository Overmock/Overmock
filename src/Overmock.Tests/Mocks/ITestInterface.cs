
namespace Overmock.Tests.Mocks
{
    public interface ITestInterface
    {
        Model Create(Model model, string name);

        Factory Factory { get; }
    }
}