using Overmock.Runtime;

namespace Overmock.Setup
{
	internal class SetupMethodOvermock<T> : ISetupOvermock<T> where T : class
	{
		protected readonly IMethodCall<T> MethodCall;

		internal SetupMethodOvermock(IMethodCall<T> methodCall)
		{
			MethodCall = methodCall;
		}

		void ISetupOvermock.ToThrow(Exception exception)
		{
			MethodCall.Throws(exception ?? throw new ArgumentNullException(nameof(exception)));
		}
	}

	internal class SetupMethodOvermock<T, TReturn> : SetupMethodOvermock<T>, ISetupOvermock<T, TReturn> where T : class
	{
		internal SetupMethodOvermock(IMethodCall<T, TReturn> methodCall) : base(methodCall)
		{
		}

		void ISetupOvermock<T, TReturn>.ToCall(Func<OverrideContext, TReturn> callback)
		{
			((IMethodCall<T, TReturn>)MethodCall).Calls(callback);
		}

		void ISetupReturn<TReturn>.ToReturn(Func<TReturn> resultProvider)
		{
			MethodCall.Returns(() => resultProvider()!);
		}

		void ISetupReturn<TReturn>.ToReturn(TReturn result)
		{
			MethodCall.Returns(() => result!);
		}
	}
}
