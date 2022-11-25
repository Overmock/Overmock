namespace Overmock.Setup
{
    internal class Override<T> : IOverride<T> where T : class
    {
        protected readonly IMethodCall<T> _methodCall;

        internal Override(MethodCall<T> methodCall)
        {
            _methodCall = methodCall;
        }

        void IOverride<T>.Calls(Action<OverrideContext<T>> callback)
        {
            _methodCall.Call(callback);
        }

        void IOverride<T>.ToThrow(Exception exception)
        {
            _methodCall.Throws(exception ?? throw new ArgumentNullException(nameof(exception)));
        }
    }

    internal class Override<T, TReturn> : Override<T>, IOverride<T, TReturn> where T : class
    {
        private Func<TReturn?> _valueProvider = () => default;

        internal Override(MethodCall<T, TReturn> methodCall) : base(methodCall)
        {
        }

        void IOverride<T, TReturn>.Calls(Func<OverrideContext<T, TReturn>, TReturn> callback)
        {
            ((IMethodCall<T, TReturn>)_methodCall).Call(callback);
        }

        void IOverride<T, TReturn>.Returns(Func<TReturn> resultProvider)
        {
            _valueProvider = resultProvider;
        }
    }
}
