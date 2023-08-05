using Overmock.Proxies;

namespace Overmock.Tests.Mocks.Methods
{
    public class GenericMethodsTestInterface : ProxyBase<IGenericMethodsTestInterface>, IGenericMethodsTestInterface
    {
        public GenericMethodsTestInterface(IOvermock target) : base()
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