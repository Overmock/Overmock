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
		IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>() where T : class;
	}

	public class GenericMethodsTestInterface : Proxy<IGenericMethodsTestInterface>, IGenericMethodsTestInterface
	{
		public GenericMethodsTestInterface(IOvermock target) : base(target)
		{
		}

		public IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>() where T : class
		{
			return default;
			//return (IEnumerable<T>)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod()!)!;
		}

		public IEnumerable<object> MethodWithNoParamsAndReturnsEnumerableOfT()
		{
			return default;
			//return (IEnumerable<T>)HandleMethodCall((MethodInfo)MethodBase.GetCurrentMethod()!)!;
		}
	}
}