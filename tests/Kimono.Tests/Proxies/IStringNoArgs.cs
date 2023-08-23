
namespace Kimono.Tests.Proxies
{
    public interface IStringNoArgs
    {
        string StringNoArgs();
    }

    public class StringNoArgsClass : IStringNoArgs
    {
        public string StringNoArgs()
        {
            return "hello, world!";
        }
    }
}
