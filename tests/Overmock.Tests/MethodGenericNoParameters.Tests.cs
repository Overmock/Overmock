using Overmock.Tests.Mocks;
using Overmock.Tests.Mocks.Methods;

namespace Overmock.Tests
{
    [TestClass]
    public partial class GenericMethodNoParametersTests
    {
        private IOvermock<IGenericMethodsTestInterface> _genericMethodsTestInterface = null!;

        [TestInitialize]
        public void Initialize()
        {
            _genericMethodsTestInterface = Overmock.Mock<IGenericMethodsTestInterface>();
        }

        [TestMethod]
        public void TestWithGenericParametersOnMethod()
        {
            var expected = new List<Model> { new Model() };

            _genericMethodsTestInterface.Mock(m => m.MethodWithNoParamsAndReturnsEnumerableOfT<Model>())
                .ToReturn(expected);

            var target = _genericMethodsTestInterface.Target;

            var actual = target.MethodWithNoParamsAndReturnsEnumerableOfT<Model>();
            Assert.IsTrue(true);
            Assert.IsNotNull(target);
            CollectionAssert.AreEqual(expected, actual.ToList());
        }
    }
}
