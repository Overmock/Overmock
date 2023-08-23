using Overmock.Tests.Mocks;

namespace Overmock.Tests
{
	public partial class GenericMethodNoParametersTests
	{
		[TestMethod]
		public void MethodWithNoParamsToReturnTest()
		{
			_genericMethodsTestInterface.Override(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
				.ToReturn(() => Enumerable.Empty<Model>());
			
			var target = _genericMethodsTestInterface.Target;

			Assert.IsNotNull(target);

			target.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();
		}

		[TestMethod]
		public void MethodWithN1ParamsToReturnTest()
		{
			_genericMethodsTestInterface.Override(m => m.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(Its.Any<Model>()))
				.ToReturn(() => Enumerable.Empty<Model>());

			var target = _genericMethodsTestInterface.Target;

			Assert.IsNotNull(target);

			
			target.MethodWith1ParamsAndReturnsEnumerableOfT<Model, Model>(new Model());
		}

		[TestMethod]
		public void MethodWithN2ParamsToReturnTest()
		{
			_genericMethodsTestInterface.Override(m => m.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(Its.Any<Model>(), Its.Any<Model>()))
				.ToReturn(() => Enumerable.Empty<Model>());

			var target = _genericMethodsTestInterface.Target;

			Assert.IsNotNull(target);


			target.MethodWith2ParamsAndReturnsEnumerableOfT<Model, Model, Model>(new Model(), new Model());
		}

		[TestMethod]
		public void MethodWithNParamsToReturnTest()
		{
			_genericMethodsTestInterface.Override(m => m.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(Its.Any<Model>(),Its.Any<Model>(), Its.Any<Model>()))
				.ToReturn(() => Enumerable.Empty<Model>());

			var target = _genericMethodsTestInterface.Target;

			Assert.IsNotNull(target);


			target.MethodWith3ParamsAndReturnsEnumerableOfT<Model, Model, Model, Model>(new Model(), new Model(), new Model());
		}
	}
}
