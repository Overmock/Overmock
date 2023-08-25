using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
    public class ISetupMocksTests
    {
        [TestMethod]
        public void ISetupMocksTestsAllowsReturnsToReturnMock()
        {
            var overmock = Overmock.MockAnyInvocation<IInterfaceNoArgs>();

            var imOvermocked = overmock.Overmock(m => m.Get())
                .ToReturnMock();

            imOvermocked.Mock(m => m.IDoNothing()).ToBeCalled();

            var target = imOvermocked.Target;

            Assert.IsNotNull(target);

            target.IDoNothing();
        }

        [TestMethod]
        public void ISetupMocksTestsAllowsReturnsToReturnMockOfT()
        {
            var overmock = Overmock.Mock<IInterfaceNoArgs>();

            var imOvermocked = overmock.Overmock(m => m.Get())
                .ToReturnMock<IInheritReturned>();

            imOvermocked.Mock(m => m.IDoNothing()).ToBeCalled();

            var target = overmock.Target;

            Assert.IsNotNull(target);

            var imtargeted = target.Get();

            ((IInheritReturned)imtargeted).IDoNothing();
        }

        [TestMethod]
        public void ISetupMocksTestsAllowsReturnsToReturnMockOfOvermockOfT()
        {
            var overmock = Overmock.MockAnyInvocation<IInterfaceNoArgs>();
            var imReturned = Overmock.MockAnyInvocation<IInheritReturned>();
            var imOvermocked = overmock.Overmock(m => m.Get())
                .ToReturnMock(imReturned);

            imOvermocked.Mock(m => m.IDoNothing()).ToBeCalled();

            var target = imOvermocked.Target;

            Assert.IsNotNull(target);

            target.IDoNothing();
        }
    }
}
