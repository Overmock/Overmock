namespace Kimono.Core
{
    public interface IInterceptor : IFluentInterface
    {
    }

    public interface IInterceptor<T> : IInterceptor
    {
        object? HandleInvocation(int methodId, IProxy<T> proxyBase, object[] parameters);
    }
}