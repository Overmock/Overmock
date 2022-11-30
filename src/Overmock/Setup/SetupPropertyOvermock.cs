namespace Overmock.Setup
{
    internal class SetupPropertyOvermock<T, TReturn> : ISetupOvermock<T, TReturn> where T : class
    {
        private readonly IPropertyCall<TReturn> _propertyCall;

        internal SetupPropertyOvermock(PropertyCall<TReturn> propertyCall)
        {
            this._propertyCall = propertyCall;
        }

        void ISetupOvermock<T, TReturn>.ToCall(Func<OverrideContext, TReturn> callback)
        {
            _propertyCall.Calls(callback);
        }

        void ISetupOvermock<T>.ToCall(Action<OverrideContext> callback)
        {
            // TODO: Verify this works as I think it will.
            _propertyCall.Calls(c =>
            {
                callback.Invoke(c);
                return default;
            });
        }

        void ISetupOvermock<T, TReturn>.ToReturn(Func<TReturn> resultProvider)
        {
            _propertyCall.Returns(() => resultProvider.Invoke());
        }

        void ISetupOvermock<T>.ToReturn(Func<T> resultProvider)
        {
            throw new NotImplementedException();
        }

        void ISetupOvermock.ToThrow(Exception exception)
        {
            _propertyCall.Throws(exception);
        }
    }
}
