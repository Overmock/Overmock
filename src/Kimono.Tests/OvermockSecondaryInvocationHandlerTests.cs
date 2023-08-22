using Kimono.Tests.Proxies;
using Overmock;

namespace Kimono.Tests
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
            var overmock = Overmocked.For<IRepository>(new TestInvocationHandler());

            Overmocked.Mock(overmock, o => o.Save(Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.Save(new Model { Id = 420 });

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void OvermockCallsGlobalHandlerTest()
        {
            Overmocked.Use(new TestInvocationHandler());

            var overmock = Overmocked.For<IRepository>();

            Overmocked.Mock(overmock, o => o.Save(Its.Any<Model>()))
                .ToBeCalled();

            var result = overmock.Save(new Model { Id = 420 });

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
