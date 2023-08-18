
namespace Kimono.Tests.Proxies
{
    public interface IMethodArgIntDoubleStringReturnsVoid
    {
        void MethodArgIntDoubleStringReturnsVoid(int id, double balance, string name);
    }

    public class MethodArgIntDoubleStringReturnsVoidClass : IMethodArgIntDoubleStringReturnsVoid
    {
        public void MethodArgIntDoubleStringReturnsVoid(int id, double balance, string name)
        {
        }
    }
}
