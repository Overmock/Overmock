
namespace Kimono.Tests.Proxies
{
    public interface IVoidNoArgs
    {
        void VoidNoArgs();
    }

    public class VoidNoArgsClass : IVoidNoArgs
    {
        public void VoidNoArgs()
        {
        }
    }

    public class ICallVoidNoArgs
    {
        public void CallVoidNoArgs(object obj)
        {
            ((IVoidNoArgs)obj).VoidNoArgs();
        }
    }
}
