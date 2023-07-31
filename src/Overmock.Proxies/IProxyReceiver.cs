namespace Overmock.Proxies
{
	public interface IProxyDecorator<T> where T : class
	{
		IProxy<T> Proxy { get; }
	}
}