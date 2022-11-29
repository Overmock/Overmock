namespace Overmock.Setup
{
    public interface ISetupOvermock
    {
        void ToThrow(Exception exception);
    }

    public interface ISetupOvermock<T> : ISetupOvermock where T : class
    {
        void Calls(Action<OverrideContext<T>> callback);

        void Returns(Func<T> resultProvider);

    }

    public interface ISetupOvermock<T, TReturn> : ISetupOvermock<T> where T : class
    {
        void Returns(Func<TReturn> resultProvider);

        void Calls(Func<OverrideContext<T, TReturn>, TReturn> callback);
    }
}
