using DataCompany.Framework;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;
using Overmock.Examples.Tests.Methods;

namespace Overmock.Examples.Tests
{
    [TestClass]
	public class UserStoryControllerTests
	{
		private IOvermock<IMethodsWithNoParameters> _testInterface = null!;

		[TestInitialize]
		public void Initialize()
		{
            _testInterface = Overmocked.Setup<IMethodsWithNoParameters>();
		}

		[TestMethod]
		public void VoidMethodWithNoParamsTest()
		{
            _testInterface.Override(t => t.VoidMethodWithNoParams());

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			target.VoidMethodWithNoParams();
		}
		
        [TestMethod]
        public void BoolMethodWithNoParamsTest()
        {
            _testInterface.Override(t => t.BoolMethodWithNoParams())
                .ToReturn(true);

            var target = _testInterface.Target;

            Assert.IsNotNull(target);

            var test = target.BoolMethodWithNoParams();

            Assert.IsTrue(test);
        }
	}
}