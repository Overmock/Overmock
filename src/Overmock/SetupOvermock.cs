
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
            _callable.Calls(action, Times.Any);
        }

        public void ToCall(Action<OvermockContext> action, Times times)
        {
            _callable.Calls(action, times);
        }

		public void ToBeCalled()
		{
            _callable.Calls(c => { }, Times.Any);
		}

		public void ToBeCalled(Times times)
		{
            _callable.Calls(c => { }, times);
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
			_returnable.Calls(callback, Times.Any);
		}

		void ISetup<T, TReturn>.ToCall(Func<OvermockContext, TReturn> callback, Times times)
        {
            _returnable.Calls(callback, times);
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
