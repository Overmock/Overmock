using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Tests.Mocks.Methods.Async
{
	public interface IAsyncMethodsWithNoParams
	{
		Task ReturnsTask();

		Task<bool> ReturnsTaskOfBoolWithNoParams() { return Task.FromResult(true); }
	}
}
