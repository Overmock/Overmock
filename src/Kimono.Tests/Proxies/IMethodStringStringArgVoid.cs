
namespace Kimono.Tests.Proxies
{
    public interface IMethodStringStringArgVoid
    {
        void MethodStringStringArgVoid(string name, string address);
    }

    public class MethodStringStringArgVoidClass : IMethodStringStringArgVoid
    {
        public void MethodStringStringArgVoid(string name, string address)
        {
        }
    }
}
