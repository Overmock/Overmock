
namespace Kimono.Tests.Proxies
{
    public interface IIntNoArgs
    {
        int IntNoArgs();
    }

    public class IntNoArgsClass : IIntNoArgs
    {
        public int IntNoArgs()
        {
            return 100;
        }
    }
}
