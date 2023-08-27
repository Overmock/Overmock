
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

    public class ICallLongArgsIntString
    {
        public object CallLongArgsIntString(object obj, int id, string str)
        {
            return ((ILongArgsIntString)obj).LongArgsIntString(id, str);
        }
    }
}
