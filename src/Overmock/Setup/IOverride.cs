namespace Overmock.Setup
{
    public interface IOverride<T> where T : class
    {
        void ToThrow(Exception exception);

        void Calls(Action<OverrideContext<T>> callback);
    }

    /// <summary>
    /// Configures overrides where each override is terminating.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public interface IOverride<T, TReturn> : IOverride<T> where T : class
    {
        void Returns(Func<TReturn> resultProvider);

        void Calls(Func<OverrideContext<T, TReturn>, TReturn> callback);
    }
}
