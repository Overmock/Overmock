namespace Overmock.Setup
{
    public interface ISetupReturn<in TReturn>
    {
        void ToReturn(Func<TReturn> resultProvider);
    }

    public interface ISetupOvermock
    {
        void ToThrow(Exception exception);
    }

    public interface ISetupOvermock<in T> : ISetupOvermock where T : class
    {
    }

    public interface ISetupOvermock<in T, in TReturn> : ISetupOvermock<T>, ISetupReturn<TReturn> where T : class
    {
        ISetupReturn<TReturn> ToCall(Func<OverrideContext, TReturn> callback);
    }
}
