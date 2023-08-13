using Kimono;

namespace Overmock.Tests.Mocks.Methods
{
    public class GenericMethodsTestInterface : ProxyBase<IGenericMethodsTestInterface>, IGenericMethodsTestInterface
    {
        public GenericMethodsTestInterface() : base()
        {
        }

		public IEnumerable<TReturn> MethodWith1ParamsAndReturnsEnumerableOfT<T, TReturn>(T param2) where TReturn : class
		{
			return default;
		}

		public IEnumerable<TReturn> MethodWith2ParamsAndReturnsEnumerableOfT<T, T2, TReturn>(T param1, T2 param2) where T2 : T where TReturn : class
		{
			return default;
		}

		public IEnumerable<TReturn> MethodWith3ParamsAndReturnsEnumerableOfT<T, T2, T3, TReturn>(T param1, T2 param2, T3 param3) where T2 : T where T3 : T where TReturn : class
		{
			return default;
		}

		public IEnumerable<TReturn> MethodWithNoParamsAndReturnsEnumerableOfT<TReturn>() where TReturn : class
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