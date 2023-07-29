using DataCompany.Framework;
using Overmock.Examples.Controllers;
using Overmock.Examples.Storage;
using Overmock.Tests.Mocks.Methods;
using System;

namespace Overmock.Examples.Tests
{
    [TestClass]
	public class MethodWithNoParamsToThrowTests
	{
		private IOvermock<IMethodsWithNoParameters> _testInterface = null!;

		[TestInitialize]
		public void Initialize()
		{
            _testInterface = Overmocked.Interface<IMethodsWithNoParameters>();
		}

		[TestMethod]
		public void VoidMethodWithNoParamsTest()
		{
			var exception = new Exception();

			_testInterface.Override(t => t.VoidMethodWithNoParams())
				.ToThrow(exception);

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			try
			{
				target.VoidMethodWithNoParams();

				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(exception, ex);
			}
		}
		
        [TestMethod]
        public void BoolMethodWithNoParamsTest()
		{
			var exception = new Exception();

			_testInterface.Override(t => t.BoolMethodWithNoParams())
				.ToThrow(exception);

			var target = _testInterface.Target;

			Assert.IsNotNull(target);

			try
			{
				target.BoolMethodWithNoParams();

				Assert.Fail();
			}
			catch (Exception ex)
			{
				Assert.AreEqual(exception, ex);
			}
		}
	}
}