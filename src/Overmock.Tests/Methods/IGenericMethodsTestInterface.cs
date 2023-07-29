namespace Overmock.Examples.Tests.Methods
{
	public interface IGenericMethodsTestInterface
	{
		IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>() where T : class;
	}
}