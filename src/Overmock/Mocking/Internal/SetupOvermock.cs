using Overmock.Mocking;
using System;

namespace Overmock
{
    /// <summary>
    /// Class SetupOvermock.
    /// Implements the <see cref="ISetup" />
    /// </summary>
    /// <seealso cref="ISetup" />
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
    /// Implements the <see cref="SetupOvermock" />
    /// Implements the <see cref="ISetup{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SetupOvermock" />
    /// <seealso cref="ISetup{T}" />
    internal sealed class SetupOvermock<T> : SetupOvermock, ISetup<T> where T : class
    {
        private readonly IOvermock<T> _overmock;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupOvermock{T}"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="callable">The callable.</param>
        internal SetupOvermock(IOvermock<T> overmock, ICallable<T> callable) : base(callable)
        {
            _overmock = overmock;
        }

        /// <summary>
        /// /
        /// </summary>
        public IOvermock<T> Overmock => _overmock;
    }

    /// <summary>
    /// Class SetupOvermock. This class cannot be inherited.
    /// Implements the <see cref="SetupOvermock" />
    /// Implements the <see cref="ISetup{T, TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="SetupOvermock" />
    /// <seealso cref="ISetup{T, TReturn}" />
    internal class SetupOvermock<T, TReturn> : SetupOvermock, ISetup<T, TReturn> where T : class
    {
        private readonly IReturnable<TReturn> _returnable;
        private readonly IOvermock<T> _overmock;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupOvermock{T, TReturn}"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="returnable">The returnable.</param>
        internal SetupOvermock(IOvermock<T> overmock, IReturnable<TReturn> returnable) : base(returnable)
        {
            _overmock = overmock;
            _returnable = returnable;
        }

        /// <summary>
        /// 
        /// </summary>
        public IOvermock<T> Overmock => _overmock;

        /// <inheritdoc />
        public void ToCall(Func<OvermockContext, TReturn> callback)
        {
            _returnable.Calls(callback, Times.Any);
        }

        /// <inheritdoc />
        public void ToCall(Func<OvermockContext, TReturn> callback, Times times)
        {
            _returnable.Calls(callback, times);
        }

        /// <inheritdoc />
        public void ToReturn(TReturn result)
        {
            _returnable.Returns(result);
        }

        /// <summary>
        /// Converts to return.
        /// </summary>
        /// <param name="returnProvider">The return provider.</param>
        public void ToReturn(Func<TReturn> returnProvider)
        {
            _returnable.Returns(returnProvider);
        }
    }

    internal sealed class SetupOvermockWithMockReturns<T, TReturn> : SetupOvermock<T, TReturn>, ISetupMocks<T, TReturn> where T : class where TReturn : class
    {
        internal SetupOvermockWithMockReturns(IOvermock<T> overmock, IReturnable<TReturn> returnable) : base(overmock, returnable)
        {
        }

        public IOvermock<TReturn> ToReturnMock()
        {
            return ToReturnMock<TReturn>();
        }

        public IOvermock<TReturn> ToReturnMock<TMock>() where TMock : class, TReturn
        {
            return ToReturnMock(new Overmock<TMock>()).AsMock<TReturn>();
        }

        public IOvermock<TMock> ToReturnMock<TMock>(IOvermock<TMock> overmock) where TMock : class, TReturn
        {
            ToReturn(() => overmock.Target);

            return overmock;
        }
    }
}
