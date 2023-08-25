namespace Overmock.Tests.Mocks.Methods.Async
{
    public interface IAsyncMethodsWithNoParams
    {
        Task ReturnsTask();

        Task<bool> ReturnsTaskOfBoolWithNoParams() { return Task.FromResult(true); }
    }
}
