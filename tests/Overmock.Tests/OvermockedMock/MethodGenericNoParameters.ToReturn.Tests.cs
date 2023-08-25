using Overmock.Tests.Mocks;

namespace Overmock.Tests.OvermockedMock
{
    public partial class GenericMethodNoParametersTests
    {
        [TestMethod]
        public void MethodWithNoParamsToReturnTest()
        {
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
                .ToReturn(() => Enumerable.Empty<Model>());

            _genericMethodsTestInterface.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();
        }

        [TestMethod]
        public void MethodWithN1ParamsToReturnTest()
        {
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            _genericMethodsTestInterface.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(new Model());
        }

        [TestMethod]
        public void MethodWithN2ParamsToReturnTest()
        {
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            _genericMethodsTestInterface.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(new Model(), new Model());
        }

        [TestMethod]
        public void MethodWithNParamsToReturnTest()
        {
            Overmock.OverMock(_genericMethodsTestInterface, m => m.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>(), Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            _genericMethodsTestInterface.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(new Model(), new Model(), new Model());
        }
    }
}
