namespace Overmock.Examples.Tests.TestCode
{
    public interface ITestInterface
    {
        string Name { get; }

        void VoidMethodWithNoParams();

        bool BoolMethodWithNoParams();

        //IEnumerable<T> MethodWithNoParamsAndReturnsEnumerableOfT<T>();
    }
}