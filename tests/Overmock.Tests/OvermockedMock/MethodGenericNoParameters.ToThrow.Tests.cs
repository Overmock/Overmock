using Overmocked.Tests.Mocks;

namespace Overmocked.Tests.OvermockedMock
{
    public partial class GenericMethodNoParametersTests
    {
        [TestMethod]
        public void MethodWithNoParamsToThrowTest()
        {
            var exception = new Exception("fail");
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception.Message, e.Message);
            }
        }

        [TestMethod]
        public void MethodWithN1ParamsToThrowTest()
        {
            var exception = new Exception("fail");
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(new Model());

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception.Message, e.Message);
            }
        }

        [TestMethod]
        public void MethodWithN2ParamsToThrowTest()
        {
            var exception = new Exception("fail");
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(new Model(), new Model());

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception.Message, e.Message);
            }
        }

        [TestMethod]
        public void MethodWithNParamsToThrowTest()
        {
            var exception = new Exception("fail");
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>(), Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(new Model(), new Model(), new Model());

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception.Message, e.Message);
            }
        }
    }
}
