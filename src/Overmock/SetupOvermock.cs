namespace Overmock
{
	/// <summary>
	/// Class SetupOvermock.
	/// Implements the <see cref="Overmock.ISetup" />
	/// </summary>
	/// <seealso cref="Overmock.ISetup" />
	internal class SetupOvermock : ISetup
    {
		/// <summary>
		/// The callable
		/// </summary>
		private readonly ICallable _callable;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetupOvermock"/> class.
		/// </summary>
		/// <param name="callable">The callable.</param>
		public SetupOvermock(ICallable callable)
        {
            _callable = callable;
        }

		/// <inheritdoc />
		public void ToThrow(Exception exception)
        {
            _callable.Throws(exception);
        }

		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="action">The action.</param>
		public void ToCall(Action<OvermockContext> action)
        {
            _callable.Calls(action, Times.Any);
        }

		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="times">The times.</param>
		public void ToCall(Action<OvermockContext> action, Times times)
        {
            _callable.Calls(action, times);
        }

		/// <inheritdoc />
		public void ToBeCalled()
		{
            _callable.Calls(c => { }, Times.Any);
		}

		/// <inheritdoc />
		public void ToBeCalled(Times times)
		{
            _callable.Calls(c => { }, times);
		}
	}
	/// <summary>
	/// Class SetupOvermock.
	/// Implements the <see cref="Overmock.SetupOvermock" />
	/// Implements the <see cref="Overmock.ISetup{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.SetupOvermock" />
	/// <seealso cref="Overmock.ISetup{T}" />
	internal class SetupOvermock<T> : SetupOvermock, ISetup<T> where T : class
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="SetupOvermock{T}"/> class.
		/// </summary>
		/// <param name="callable">The callable.</param>
		internal SetupOvermock(ICallable<T> callable) : base(callable)
        {
        }
    }

	/// <summary>
	/// Class SetupOvermock. This class cannot be inherited.
	/// Implements the <see cref="Overmock.SetupOvermock" />
	/// Implements the <see cref="Overmock.ISetup{T, TReturn}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TReturn">The type of the t return.</typeparam>
	/// <seealso cref="Overmock.SetupOvermock" />
	/// <seealso cref="Overmock.ISetup{T, TReturn}" />
	internal sealed class SetupOvermock<T, TReturn> : SetupOvermock, ISetup<T, TReturn> where T : class
    {
		/// <summary>
		/// The returnable
		/// </summary>
		private readonly IReturnable<TReturn> _returnable;

		/// <summary>
		/// Initializes a new instance of the <see cref="SetupOvermock{T, TReturn}"/> class.
		/// </summary>
		/// <param name="returnable">The returnable.</param>
		internal SetupOvermock(IReturnable<TReturn> returnable) : base(returnable)
        {
            _returnable = returnable;
		}

		/// <inheritdoc />
		void ISetup<T, TReturn>.ToCall(Func<OvermockContext, TReturn> callback)
		{
			_returnable.Calls(callback, Times.Any);
		}

		/// <inheritdoc />
		void ISetup<T, TReturn>.ToCall(Func<OvermockContext, TReturn> callback, Times times)
        {
            _returnable.Calls(callback, times);
        }

		/// <inheritdoc />
		void ISetupReturn<TReturn>.ToReturn(TReturn result)
        {
            _returnable.Returns(result);
        }

		/// <summary>
		/// Converts to return.
		/// </summary>
		/// <param name="returnProvider">The return provider.</param>
		void ISetupReturn<TReturn>.ToReturn(Func<TReturn> returnProvider)
        {
            _returnable.Returns(returnProvider);
        }
    }
}
