using Kimono;
using Overmocked.Tests.Mocks;
using Overmocked.Tests.Mocks.Methods;

namespace Overmocked.Tests
{
    [TestClass]
    public class OvermockSecondaryInvocationHandlerTests
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        [TestMethod]
        public void OvermockCallsSecondaryHandlerTest()
        {
            var overmock = Overmock.Mock<IMethodsWith2Parameters>(new TestInvocationHandler());

            Overmock.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.Target.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OvermockCallsGlobalHandlerTest()
        {
            Overmock.Use(new TestInvocationHandler());

            var overmock = Overmock.Mock<IMethodsWith2Parameters>();

            Overmock.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.Target.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);

            Overmock.Use(null);
        }

        private class TestInvocationHandler : IInvocationHandler
        {
            public void Handle(IInvocationContext context)
            {
                context.ReturnValue = true;
            }
        }
    }
}
