namespace Kimono
{
    public interface IProxyFactory
    {
        IDelegateFactory MethodFactory { get; }

        T CreateInterfaceProxy<T>(IInterceptor<T> interceptor) where T : class;
    }
}
