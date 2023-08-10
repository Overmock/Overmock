namespace Overmock.Tests.Mocks.Methods
{
    public interface IGenericMethodsTestInterface
    {
        IEnumerable<TReturn> MethodWithNoParamsAndReturnsEnumerableOfT<TReturn>() where TReturn : class;
		IEnumerable<TReturn> MethodWith1ParamsAndReturnsEnumerableOfT<T, TReturn>(T param2) where TReturn : class;
		IEnumerable<TReturn> MethodWith2ParamsAndReturnsEnumerableOfT<T, T2, TReturn>(T param1, T2 param2) where T2 : T where TReturn : class;
		IEnumerable<TReturn> MethodWith3ParamsAndReturnsEnumerableOfT<T, T2, T3, TReturn>(T param1, T2 param2, T3 param3) where T2 : T where T3 : T where TReturn : class;
	}
}