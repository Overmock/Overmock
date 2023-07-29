namespace Overmock.Tests.Mocks.Methods
{
    public interface IGenericMethodsTestInterface
    {
        IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>() where T : class;
    }
}