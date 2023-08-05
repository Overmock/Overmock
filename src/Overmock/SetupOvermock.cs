
namespace Overmock
{
	internal class SetupOvermock : ISetup
    {
        private readonly ICallable _callable;

        public SetupOvermock(ICallable callable)
        {
            _callable = callable;
        }

        public void ToThrow(Exception exception)
        {
            _callable.Throws(exception);
        }

        public void ToCall(Action<OvermockContext> action)
        {
            _callable.Calls(action);
        }

		public void ToBeCalled()
		{
            _callable.Calls(c => { });
		}
	}
    internal class SetupOvermock<T> : SetupOvermock, ISetup<T> where T : class
    {
        internal SetupOvermock(ICallable<T> callable) : base(callable)
        {
        }
    }

    internal class SetupOvermock<T, TReturn> : SetupOvermock, ISetup<T, TReturn> where T : class
    {
        private readonly IReturnable<TReturn> _returnable;

        internal SetupOvermock(IReturnable<TReturn> returnable) : base(returnable)
        {
            _returnable = returnable;
        }

        void ISetup<T, TReturn>.ToCall(Func<OvermockContext, TReturn> callback)
        {
            _returnable.Calls(callback);
        }

        void ISetup<T>.ToCall(Action<OvermockContext> action)
        {
            _returnable.Calls(action);
        }

        void ISetupReturn<TReturn>.ToReturn(TReturn result)
        {
            _returnable.Returns(result);
        }

        void ISetupReturn<TReturn>.ToReturn(Func<TReturn> returnProvider)
        {
            _returnable.Returns(returnProvider);
        }
    }
}
