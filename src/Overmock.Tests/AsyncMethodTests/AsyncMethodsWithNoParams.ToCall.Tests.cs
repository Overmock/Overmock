using Microsoft.VisualStudio.TestTools.UnitTesting;
using Overmock.Tests.Mocks.Methods.Async;

namespace Overmock.Tests
{
	public partial class AsyncMethodsWithNoParamsTests
	{
		[TestMethod]
		public async Task AsyncMethodWithNoParamsToCallTest()
		{
			_overmock.Override(a  => a.ReturnsTask())
				.ToCall(c => Task.CompletedTask);

			var overmock = _overmock.Target;

			await overmock.ReturnsTask();
		}

		[TestMethod]
		public async Task AsyncBoolMethodWithNoParamsToCallTest()
		{
			_overmock.Override(a => a.ReturnsTaskOfBoolWithNoParams())
				.ToCall(c => Task.FromResult(true));

			var overmock = _overmock.Target;

			var result = await overmock.ReturnsTaskOfBoolWithNoParams();

			Assert.IsTrue(result);
		}
	}
}
