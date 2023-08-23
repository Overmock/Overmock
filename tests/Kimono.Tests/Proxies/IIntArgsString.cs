
namespace Kimono.Tests.Proxies
{
    public interface IIntArgsString
    {
        int IntArgsString(string message);
    }

    public class IntArgsStringClass : IIntArgsString
    {
        public int IntArgsString(string message)
        {
            return 100;
        }
    }
}
