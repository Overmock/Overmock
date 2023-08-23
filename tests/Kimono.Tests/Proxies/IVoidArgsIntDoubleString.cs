
namespace Kimono.Tests.Proxies
{
    public interface IVoidArgsIntDoubleString
    {
        void VoidArgsIntDoubleString(int id, double balance, string name);
    }

    public class VoidArgsIntDoubleStringClass : IVoidArgsIntDoubleString
    {
        public void VoidArgsIntDoubleString(int id, double balance, string name)
        {
        }
    }
}
