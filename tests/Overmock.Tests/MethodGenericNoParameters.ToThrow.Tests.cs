using Overmock.Tests.Mocks;

namespace Overmock.Tests
{
    public partial class GenericMethodNoParametersTests
    {
        [TestMethod]
        public void MethodWithNoParamsToThrowTest()
        {
            var exception = new Exception("fail");
            _genericMethodsTestInterface.Mock(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.Target.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();

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
            _genericMethodsTestInterface.Mock(m => m.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.Target.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(new Model());

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
            _genericMethodsTestInterface.Mock(m => m.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.Target.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(new Model(), new Model());

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
            _genericMethodsTestInterface.Mock(m => m.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>(), Its.Any<Model>()))
                .ToThrow(exception);
            try
            {
                _genericMethodsTestInterface.Target.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(new Model(), new Model(), new Model());

                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception.Message, e.Message);
            }
        }
    }
}
