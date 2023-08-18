
namespace Kimono.Tests.Proxies
{
    public interface IMethodArgIntStringReturnsVoid
    {
        void MethodArgIntStringReturnsVoid(int id, string name);
    }

    public class MethodArgIntStringReturnsVoidClass : IMethodArgIntStringReturnsVoid
    {
        public void MethodArgIntStringReturnsVoid(int id, string name)
        {
        }
    }
}
