namespace Overmock.Runtime.Proxies
{
	internal class DelegateProxyMarshaller : ProxyMarshaller
    {
        public DelegateProxyMarshaller(IOvermock target, Action<SetupArgs>? argsProvider) : base(target, argsProvider)
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
