namespace Overmock.Setup
{
    public interface ISetupOvermock<T> where T : class
    {
        void ToThrow(Exception exception);

        void Calls(Action<OverrideContext<T>> callback);
    }

    public interface ISetupOvermock<T, TReturn> : ISetupOvermock<T> where T : class
    {
        void Returns(Func<TReturn> resultProvider);

        void Calls(Func<OverrideContext<T, TReturn>, TReturn> callback);
    }
}
