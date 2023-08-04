namespace Overmock.Proxies
{
    public interface IProxyGenerator
    {
        object GenerateProxy(IInterceptor interceptor);
	}

	public interface IProxyGenerator<T> : IProxyGenerator where T : class
	{
		T GenerateProxy(IInterceptor<T> interceptor);
	}
}