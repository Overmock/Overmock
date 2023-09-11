using Overmocked.Tests.Mocks.Mixed;

namespace Overmocked.Tests
{
    public partial class MixedMethodsAndPropertiesTests
    {
        private const string _helloWorld = "hello world";

        [TestMethod]
        public void ProxyCallsMethodWithParametersToReturn()
        {
            var overmock = Overmock.Mock<IInterfaceWithBothMethodsAndProperties>();
            overmock.Mock(t => t.MethodWithReturn(Its.Any<string>()))
                .ToReturn(() => _helloWorld);

            var actual = overmock.Target.MethodWithReturn(_helloWorld);

            Assert.AreEqual(_helloWorld, actual);
        }

        [TestMethod]
        public void ProxyCallsDoSomethingToBeCalled()
        {
            var overmock = Overmock.Mock<IInterfaceWithBothMethodsAndProperties>();
            overmock.Mock(t => t.DoSomething(Its.Any<string>()))
                .ToBeCalled();

            overmock.Target.DoSomething(_helloWorld);

            overmock.Verify();
        }

        [TestMethod]
        public void ProxyCallsMethodWithParametersToThrow()
        {
            var exception = new Exception("fail");

            var overmock = Overmock.Mock<IInterfaceWithBothMethodsAndProperties>();
            overmock.Mock(t => t.MethodWithReturn(Its.Any<string>()))
                .ToThrow(exception);

            try
            {
                var actual = overmock.Target.MethodWithReturn(_helloWorld);

                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.AreEqual(exception.Message, ex.Message);
            }
        }
    }
}
