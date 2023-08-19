
namespace Kimono.Tests.Proxies
{
    public interface IVoidArgsStringString
    {
        void VoidArgsStringString(string name, string address);
    }

    public class VoidArgsStringStringClass : IVoidArgsStringString
    {
        public void VoidArgsStringString(string name, string address)
        {
        }
    }
}
