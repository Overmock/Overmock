namespace Overmock.Proxies.Internal
{
    internal class DelegateProxyMarshaller : ProxyMarshaller
    {
        public DelegateProxyMarshaller(IInterceptor interceptor) : base(interceptor)
        {
        }

        protected override IMarshallerContext CreateContext()
        {
            throw new NotImplementedException();
        }

        protected override object MarshalCore(IMarshallerContext marshallerContext)
        {
            throw new NotImplementedException();
        }
    }
}
