using Kimono.Proxies;

namespace Kimono.Tests.Proxies
{
    public interface IInterface
    {
        string Name { get; }

        string DoSomething(string name);

        string MethodWithReturn(string name, object param);
    }

    public class InterfaceImpl : ProxyBase<IInterface>, IInterface
    {
        public InterfaceImpl() : base(new ProxyContext(), null)
        {
        }

        public string Name => throw new NotImplementedException();

        public string DoSomething(string name)
        {
            const int methodId = 90001;
            return (string)HandleMethodCall(methodId, Type.EmptyTypes, name)!;
        }

        public string MethodWithReturn(string name, object param)
        {
            throw new NotImplementedException();
        }
    }

    internal class InterfaceProxy : Interceptor<IInterface>
    {
        public InterfaceProxy(IInterface target) : base(target)
        {
        }

        protected override void MemberInvoked(IInvocationContext context)
        {
        }
    }
}
