namespace Overmock.Proxies.Internal
{
    internal class DelegateProxyFactory : ProxyFactory
    {
        public DelegateProxyFactory(IInterceptor interceptor, IProxyCache cache) : base(interceptor, cache)
        {
        }

        protected override IProxyBuilderContext CreateContext()
        {
            throw new NotImplementedException();
        }

        protected override object CreateCore(IProxyBuilderContext marshallerContext)
        {
            throw new NotImplementedException();
        }
    }
}
