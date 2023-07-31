using Overmock.Tests.Mocks.Mixed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Tests
{
	[TestClass]
	public partial class MixedMethodsAndPropertiesTests
	{
		[TestMethod]
		public void ProxyCallsMethodWithParameters()
		{
			var called = false;

			var overmock = Overmocked.Interface<IInterfaceWithBothMethodsAndProperties>();
			overmock.Override(t => t.MethodWithReturn(Its.Any<string>()))
				.ToCall(c => {
					called = true;
					return c.Get<string>("name")!;
				});

			var actual = overmock.Target.MethodWithReturn("hello world");

			Assert.IsTrue(called);
			Assert.AreEqual("hello world", actual);
		}
	}
}
