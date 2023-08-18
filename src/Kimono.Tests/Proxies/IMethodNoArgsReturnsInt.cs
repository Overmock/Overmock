
namespace Kimono.Tests.Proxies
{
    public interface IMethodNoArgsReturnsInt
    {
        int MethodNoArgsInt();
    }

    public class MethodNoArgsIntClass : IMethodNoArgsReturnsInt
    {
        public int MethodNoArgsInt()
        {
            return 100;
        }
    }
}
