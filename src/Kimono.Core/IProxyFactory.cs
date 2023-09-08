using System.Reflection.Emit;

namespace Kimono.Core
{
    public interface IProxyFactory
    {
        IDelegateFactory DelegateFactory { get; }

        T CreateInterfaceProxy<T>() where T : class;
    }
}
