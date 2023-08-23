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
            var overmock = Overmocked.For<IMethodsWith2Parameters>(new TestInvocationHandler());

            Overmocked.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OvermockCallsGlobalHandlerTest()
        {
            Overmocked.Use(new TestInvocationHandler());

			var overmock = Overmocked.For<IMethodsWith2Parameters>();

			Overmocked.Mock(overmock, o => o.BoolMethodWithStringAndModel(Its.Any<string>(), Its.Any<Model>()))
				.ToBeCalled();

			var result = overmock.BoolMethodWithStringAndModel("hello", new Model { Id = 420 });

            Assert.IsTrue(result);

            Overmocked.Use(null);
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
