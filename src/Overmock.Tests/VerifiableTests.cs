using Overmock.Tests.Mocks;

namespace Overmock.Tests
{
    [TestClass]
    public class VerifiableTests
    {
        private IOvermock<ITestInterface> _interface;
        private IOvermock<Factory> _factory;
        private IOvermock<IProvider> _provider;

        private Controller _controller;

        [TestInitialize]
        public void Initialize()
        {
            _interface = Overmocked.Setup<ITestInterface>();
            _provider = Overmocked.Setup<IProvider>();
            _factory = Overmocked.Setup<Factory>(args =>
            {
                args.Args = _provider.Object;
            });


            _factory.Override(f => f.GoDoYourWork())
                .ToThrow(new NullReferenceException());

            _factory.Override(f => f.GoDoWork()).Calls(c =>
            {
                var work = new Work("I overwrote the original method to call this.");
            });

            // TODO: Add and It.IsAny<T> of some sort for mocking parameters.

            _factory.Override(f => f.GoDoYourWork(/* o1 T.Val<Object1> */null, /* o2 T.Val<Object2> */null)).Calls(c =>
            {
                var obj1 = c.Get<object>("o1"); // f.GoDoYourWork o1
                var obj2 = c.Get<object>("o2"); // f.GoDoYourWork o2

                // TODO: I don't know if we should allow them to return here?
                return new Work("I overwrote the original result");
            });
            
            _provider.Override(i => i.GetName())
                .ToThrow(new NullReferenceException());

            // TODO: Properties need handled
            //_interface.Override(i => i.Factory).Returns(() => _factory.Object);
        }

        [TestMethod]
        public void TestMethod1()
        {
            // Arrange
            _controller = new Controller(_interface.Object, _factory.Object, _provider.Object);

            // Act
            var result = _controller.DoSomeWork();

            Assert.AreEqual("Object reference not set to an instance of an object.", result.Result);

            // Assert
            Overmocked.Verify();
        }
    }
}

#region Currently Generated Types
// Currently the framework generates these classes
namespace OvermockGenerated
{
    using System;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Overmock.Tests.Mocks;

    public class Factory_710880f38eed445f9638bb8e77ce9580 : Factory
    {
        public Factory_710880f38eed445f9638bb8e77ce9580(IProvider provider) : base(provider)
        {
        }

        internal new IDidWork<String> GoDoYourWork()
        {
            base.GoDoYourWork();
            throw new System.NullReferenceException("Object reference not set to an instance of an object.");
        }
    }
}
#endregion