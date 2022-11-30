namespace Overmock.Setup
{
    public interface ISetupOvermock
    {
        void ToThrow(Exception exception);
    }

    public interface ISetupOvermock<T> : ISetupOvermock where T : class
    {
        void ToCall(Action<OverrideContext> callback);

        void ToReturn(Func<T> resultProvider);

    }

    public interface ISetupOvermock<T, TReturn> : ISetupOvermock<T> where T : class
    {
        void ToCall(Func<OverrideContext, TReturn> callback);

        void ToReturn(Func<TReturn> resultProvider);
    }
}
