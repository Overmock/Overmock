﻿using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests.OvermockedMock
{
    [TestClass]
	public partial class GenericMethodNoParametersTests
	{
		private IGenericMethodsTestInterface _genericMethodsTestInterface = null!;

		[TestInitialize]
		public void Initialize()
		{
			_genericMethodsTestInterface = Overmocked.For<IGenericMethodsTestInterface>();
		}

		[TestMethod]
		public void TestWithGenericParametersOnMethod()
		{
			var expected = new List<Model> { new  Model() };

            Overmocked.Mock(_genericMethodsTestInterface, m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
				.ToReturn(expected);

			var actual = _genericMethodsTestInterface.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();
			Assert.IsTrue(true);
			Assert.IsNotNull(actual);
			CollectionAssert.AreEqual(expected, actual.ToList());
		}
	}
}