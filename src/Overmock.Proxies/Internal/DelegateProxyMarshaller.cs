namespace Overmock.Proxies.Internal
{
    internal class DelegateProxyMarshaller : ProxyFactory
    {
        public DelegateProxyMarshaller(IInterceptor interceptor) : base(interceptor)
        {
        }

        protected override IMarshallerContext CreateContext()
        {
            throw new NotImplementedException();
        }

        protected override object CreateCore(IMarshallerContext marshallerContext)
        {
            throw new NotImplementedException();
        }
    }
}
