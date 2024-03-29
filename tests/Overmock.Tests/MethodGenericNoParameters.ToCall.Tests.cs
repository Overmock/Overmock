﻿using Overmocked.Tests.Mocks;

namespace Overmocked.Tests
{
    public partial class GenericMethodNoParametersTests
    {
        [TestMethod]
        public void MethodWithNoParamsToCallTest()
        {
            _genericMethodsTestInterface.Mock(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
                .ToReturn(() => Enumerable.Empty<Model>());

            var target = _genericMethodsTestInterface.Target;

            Assert.IsNotNull(target);

            target.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();
        }

        [TestMethod]
        public void MethodWithN1ParamsToCallTest()
        {
            _genericMethodsTestInterface.Mock(m => m.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            var target = _genericMethodsTestInterface.Target;

            Assert.IsNotNull(target);


            target.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(new Model());
        }

        [TestMethod]
        public void MethodWithN2ParamsToCallTest()
        {
            _genericMethodsTestInterface.Mock(m => m.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            var target = _genericMethodsTestInterface.Target;

            Assert.IsNotNull(target);


            target.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(new Model(), new Model());
        }

        [TestMethod]
        public void MethodWithNParamsToCallTest()
        {
            _genericMethodsTestInterface.Mock(m => m.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>(), Its.Any<Model>()))
                .ToReturn(() => Enumerable.Empty<Model>());

            var target = _genericMethodsTestInterface.Target;

            Assert.IsNotNull(target);


            target.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(new Model(), new Model(), new Model());
        }
    }
}
