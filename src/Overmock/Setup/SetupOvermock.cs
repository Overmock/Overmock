namespace Overmock.Setup
{
    internal class SetupOvermock<T> : ISetupOvermock<T> where T : class
    {
        protected readonly IMethodCall<T> _methodCall;

        internal SetupOvermock(MethodCall<T> methodCall)
        {
            _methodCall = methodCall;
        }

        void ISetupOvermock<T>.Calls(Action<OverrideContext<T>> callback)
        {
            _methodCall.Call(callback);
        }

        void ISetupOvermock<T>.Returns(Func<T> resultProvider)
        {
            _methodCall.Returns(resultProvider);
        }

        void ISetupOvermock.ToThrow(Exception exception)
        {
            _methodCall.Throws(exception ?? throw new ArgumentNullException(nameof(exception)));
        }
    }

    internal class SetupOverride<T, TReturn> : SetupOvermock<T>, ISetupOvermock<T, TReturn> where T : class
    {
        private Func<TReturn?> _valueProvider = () => default;

        internal SetupOverride(MethodCall<T, TReturn> methodCall) : base(methodCall)
        {
        }

        void ISetupOvermock<T, TReturn>.Calls(Func<OverrideContext<T, TReturn>, TReturn> callback)
        {
            ((IMethodCall<T, TReturn>)_methodCall).Call(callback);
        }

        void ISetupOvermock<T, TReturn>.Returns(Func<TReturn> resultProvider)
        {
            _valueProvider = resultProvider;
        }
    }
}
