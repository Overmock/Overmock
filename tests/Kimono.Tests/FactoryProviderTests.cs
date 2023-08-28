using Kimono.Proxies;

namespace Kimono.Tests
{
    [TestClass]
    public class FactoryProviderTests
    {
        [TestMethod]
        public void UseExpressionDelegatesConfiguresTheSystemToUseExpressionDelegateFactory()
        {
            var delegateFactory = FactoryProvider.UseExpressionDelegates();

            Assert.IsInstanceOfType<ExpressionDelegateFactory>(delegateFactory);
        }

        [TestMethod]
        public void UseDynamicMethodDelegatesConfiguresTheSystemToUseDynamicMethodDelegateFactory()
        {
            var delegateFactory = FactoryProvider.UseDynamicMethodDelegates();

            Assert.IsInstanceOfType<DynamicMethodDelegateFactory>(delegateFactory);
        }
    }
}
