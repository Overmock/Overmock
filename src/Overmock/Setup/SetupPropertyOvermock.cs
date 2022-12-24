using Overmock.Runtime;

namespace Overmock.Setup
{
    internal class SetupPropertyOvermock<T, TReturn> : ISetupOvermock<T, TReturn> where T : class
    {
        private readonly IPropertyCall<TReturn> _propertyCall;

        internal SetupPropertyOvermock(PropertyCall<TReturn> propertyCall)
        {
            _propertyCall = propertyCall;
        }

        ISetupReturn<TReturn> ISetupOvermock<T, TReturn>.ToCall(Func<OverrideContext, TReturn> callback)
        {
            _propertyCall.Calls(callback);
            return this;
        }

        void ISetupReturn<TReturn>.ToReturn(Func<TReturn> resultProvider)
        {
            _propertyCall.Returns(() => resultProvider()!);
        }

        void ISetupOvermock.ToThrow(Exception exception)
        {
            _propertyCall.Throws(exception);
        }
    }
}
