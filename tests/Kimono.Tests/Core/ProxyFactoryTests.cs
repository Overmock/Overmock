using Kimono.Tests.Proxies;

namespace Kimono.Tests.Core
{
    [TestClass]
    public class ProxyFactoryTests
    {
        [TestMethod]
        public void ProxyFactoryTest()
        {
            var factory = ProxyFactory.Create();

            var proxy = factory.CreateInterfaceProxy(new Interceptor<IVoidNoArgs>());

            proxy.VoidNoArgs();
        }
    }
}
