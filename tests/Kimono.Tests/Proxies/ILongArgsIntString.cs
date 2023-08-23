
namespace Kimono.Tests.Proxies
{
    public interface ILongArgsIntString
    {
        long LongArgsIntString(int discount, string status);
    }

    public class LongArgsIntStringClass : ILongArgsIntString
    {
        public long LongArgsIntString(int discount, string status)
        {
            return 420;
        }
    }
}
