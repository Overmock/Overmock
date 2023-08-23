using Overmock.Tests.Mocks.Methods.Async;

namespace Overmock.Tests
{
	[TestClass]
	public partial class AsyncMethodsWithNoParamsTests
	{
		private readonly IOvermock<IAsyncMethodsWithNoParams> _overmock = new Overmock<IAsyncMethodsWithNoParams>();

		[TestInitialize]
		public void Initialize()
		{

		}
	}
}
