using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Tests.Mocks.Mixed
{
	public interface IInterfaceWithBothMethodsAndProperties
	{
		string Name { get; }

		void DoSomething(string name);

		string MethodWithReturn(string name);
	}
}
