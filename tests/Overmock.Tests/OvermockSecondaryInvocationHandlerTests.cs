using Kimono;
using Overmock;
using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
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
            var overmock = Over.Mock<IMethodsWith2Parameters>(new TestInvocationHandler());

            Over.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.Target.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OvermockCallsGlobalHandlerTest()
        {
            Over.Use(new TestInvocationHandler());

			var overmock = Over.Mock<IMethodsWith2Parameters>();

            Over.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
				.ToBeCalled();

			var result = overmock.Target.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);

            Over.Use(null);
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
