using Overmock.Tests.Mocks.Mixed;

namespace Overmock.Tests
{
    [TestClass]
    public partial class MixedMethodsAndPropertiesTests
    {
        [TestMethod]
        public void ProxyCallsMethodWithParameters()
        {
            var called = false;

            var overmock = Over.Mock<IInterfaceWithBothMethodsAndProperties>();
            overmock.Mock(t => t.MethodWithReturn(Its.Any<string>()))
                .ToCall(c => {
                    called = true;
                    return c.Get<string>("name")!;
                });

            var actual = overmock.Target.MethodWithReturn("hello world");

            Assert.IsTrue(called);
            Assert.AreEqual("hello world", actual);
        }
    }
}
