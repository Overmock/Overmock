
namespace Kimono.Tests.Proxies
{
    public interface IMethodStringArgReturnsInt
    {
        int MethodStringArgReturnsInt(string message);
    }

    public class MethodStringArgReturnsIntClass : IMethodStringArgReturnsInt
    {
        public int MethodStringArgReturnsInt(string message)
        {
            return 100;
        }
    }
}
