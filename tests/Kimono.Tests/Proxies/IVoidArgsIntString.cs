
namespace Kimono.Tests.Proxies
{
    public interface IVoidArgsIntString
    {
        void VoidArgsIntString(int id, string name);
    }

    public class VoidArgsIntStringClass : IVoidArgsIntString
    {
        public void VoidArgsIntString(int id, string name)
        {
        }
    }
}
