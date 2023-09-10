using Kimono.Core;
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

            factory.CreateInterfaceProxy(new Kimono.Core.Interceptor<IRepository>());
        }
    }
}
