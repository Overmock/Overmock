namespace Overmock.Proxies.Internal
{
    internal class DelegateProxyFactory : ProxyFactory
    {
        public DelegateProxyFactory(IProxyCache cache) : base(cache)
        {
        }

        protected override IProxyBuilderContext CreateContext(IInterceptor interceptor)
        {
            throw new NotImplementedException();
        }

        protected override IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext marshallerContext)
        {
            throw new NotImplementedException();
        }
    }
}
