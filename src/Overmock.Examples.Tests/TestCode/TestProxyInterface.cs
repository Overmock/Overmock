using System.Reflection;
using Overmock.Runtime.Proxies;

namespace Overmock.Examples.Tests.TestCode
{
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

        //IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>();
    }
}