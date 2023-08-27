using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public class ISetupMocksTests
    {
        [TestMethod]
        public void ISetupMocksTestsAllowsReturnsToReturnMock()
        {
            var overmock = Overmock.AnyInvocation<IInterfaceNoArgs>();

            var imOvermocked = Overmock.OverMock(overmock, m => m.Get())
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
            var imOvermocked = overmock.OverMock(m => m.Get())
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
            var overmock = Overmock.AnyInvocation<IInterfaceNoArgs>();
            var imReturned = Overmock.Mock<IInheritReturned>();
            var imOvermocked = Overmock.OverMock(overmock, m => m.Get())
                .ToReturnMock(imReturned);

            imOvermocked.Mock(m => m.IDoNothing()).ToBeCalled();

            var target = imOvermocked.Target;

            Assert.IsNotNull(target);

            target.IDoNothing();
        }
    }
}
