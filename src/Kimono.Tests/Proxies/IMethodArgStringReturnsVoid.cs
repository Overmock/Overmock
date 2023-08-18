
namespace Kimono.Tests.Proxies
{
    public interface IMethodArgStringReturnsVoid
    {
        void MethodArgStringReturnsVoid(string name);
    }

    public class MethodArgStringReturnsVoidClass : IMethodArgStringReturnsVoid
    {
        public void MethodArgStringReturnsVoid(string name)
        {
        }
    }
}
