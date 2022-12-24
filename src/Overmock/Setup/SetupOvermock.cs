using Overmock.Runtime;

namespace Overmock.Setup
{
    internal class SetupOvermock<T> : ISetupOvermock<T> where T : class
    {
        protected readonly IMethodCall<T> MethodCall;

        internal SetupOvermock(MethodCall<T> methodCall)
        {
            MethodCall = methodCall;
        }

        void ISetupOvermock.ToThrow(Exception exception)
        {
            MethodCall.Throws(exception ?? throw new ArgumentNullException(nameof(exception)));
        }
    }

    internal class SetupOverride<T, TReturn> : SetupOvermock<T>, ISetupOvermock<T, TReturn> where T : class
    {
        internal SetupOverride(MethodCall<T, TReturn> methodCall) : base(methodCall)
        {
        }

        ISetupReturn<TReturn> ISetupOvermock<T, TReturn>.ToCall(Func<OverrideContext, TReturn> callback)
        {
            ((IMethodCall<T, TReturn>)MethodCall).Call(callback);
            return this;
        }

        void ISetupReturn<TReturn>.ToReturn(Func<TReturn> resultProvider)
        {
            MethodCall.Returns(() => resultProvider()!);
        }
    }
}
