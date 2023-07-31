using Overmock.Runtime.Proxies;

namespace Overmock.Tests.Mocks.Methods
{
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