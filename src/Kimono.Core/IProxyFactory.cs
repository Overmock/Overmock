using System.Reflection.Emit;

namespace Kimono.Core
{
    public interface IProxyFactory
    {
        IDelegateFactory MethodFactory { get; }

        T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class;
    }
}
