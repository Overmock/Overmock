
namespace Kimono.Tests.Proxies
{
    public interface IVoidArgsString
    {
        void VoidArgsString(string name);
    }

    public class VoidArgsStringClass : IVoidArgsString
    {
        public void VoidArgsString(string name)
        {
        }
    }
}
