using Overmock.Runtime.Proxies;
using System.Reflection;

namespace Overmock.Examples.Tests.TestCode
{
    public interface ITestInterface
    {
        string Name { get; }

        void VoidMethodWithNoParams();

        bool BoolMethodWithNoParams();
	}
	public class TestProxyInterface : Proxy<ITestInterface>, ITestInterface
	{
		public string Name { get; set; }

		public TestProxyInterface(IOvermock target) : base(target)
		{
		}

		public void VoidMethodWithNoParams()
		{
			HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());
		}

		public bool BoolMethodWithNoParams()
		{
			return (bool)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());
		}
	}

	public interface IGenericMethodsTestInterface
	{
		IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>();
	}

	public class GenericMethodsTestInterface : Proxy<IGenericMethodsTestInterface>, IGenericMethodsTestInterface
	{
		public GenericMethodsTestInterface(IOvermock target) : base(target)
		{
		}

		public IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>()
		{
			return (IEnumerable<T>)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod());
		}
	}
}