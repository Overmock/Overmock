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

		void ISetupOvermock<T, TReturn>.ToCall(Func<RuntimeContext, TReturn> callback)
		{
			_propertyCall.Calls(callback);
		}

		void ISetupReturn<TReturn>.ToReturn(Func<TReturn> resultProvider)
		{
			_propertyCall.Returns(() => resultProvider()!);
		}

		void ISetupReturn<TReturn>.ToReturn(TReturn result)
		{
			_propertyCall.Returns(() => result!);
		}

		void ISetupOvermock.ToThrow(Exception exception)
		{
			_propertyCall.Throws(exception);
		}
	}
}
