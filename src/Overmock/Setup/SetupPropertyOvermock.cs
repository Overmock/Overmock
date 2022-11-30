namespace Overmock.Setup
{
    internal class SetupPropertyOvermock<T, TReturn> : ISetupOvermock<T, TReturn> where T : class
    {
        private readonly IPropertyCall<TReturn> _propertyCall;

        internal SetupPropertyOvermock(PropertyCall<TReturn> propertyCall)
        {
            this._propertyCall = propertyCall;
        }

        void ISetupOvermock<T, TReturn>.Calls(Func<OverrideContext<T, TReturn>, TReturn> callback)
        {
            throw new NotImplementedException();
        }

        void ISetupOvermock<T>.Calls(Action<OverrideContext<T>> callback)
        {
            throw new NotImplementedException();
        }

        void ISetupOvermock<T, TReturn>.Returns(Func<TReturn> resultProvider)
        {
            _propertyCall.Returns(() => resultProvider.Invoke());
        }

        void ISetupOvermock<T>.Returns(Func<T> resultProvider)
        {
            throw new NotImplementedException();
        }

        void ISetupOvermock.ToThrow(Exception exception)
        {
            _propertyCall.Throws(exception);
        }
    }
}
