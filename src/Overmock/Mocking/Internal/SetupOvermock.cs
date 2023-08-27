using Overmock.Mocking;
using Overmock.Mocking.Internal;
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
    }

    /// <summary>
    /// Class SetupOvermock.
    /// Implements the <see cref="SetupOvermock" />
    /// Implements the <see cref="ISetup{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="SetupOvermock" />
    /// <seealso cref="ISetup{T}" />
    internal class SetupOvermock<T> : SetupOvermock, ISetup<T> where T : class
    {
        private readonly IOvermock<T> _overmock;

        /// <summary>
        /// The callable
        /// </summary>
        private readonly ICallable _callable;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupOvermock"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="callable">The callable.</param>
        public SetupOvermock(IOvermock<T> overmock, ICallable callable)
        {
            _overmock = overmock;
            _callable = callable;
        }

        /// <summary>
        /// The target overmock be configured.
        /// </summary>
        public IOvermock<T> Overmock => _overmock;

        /// <inheritdoc />
        public IOvermock<T> ToThrow(Exception exception)
        {
            _callable.Throws(exception);
            return _overmock;
        }

        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="action">The action.</param>
        public IOvermock<T> ToCall(Action<OvermockContext> action)
        {
            _callable.Calls(action, Times.Any);
            return _overmock;
        }

        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="times">The times.</param>
        public IOvermock<T> ToCall(Action<OvermockContext> action, Times times)
        {
            _callable.Calls(action, times);
            return _overmock;
        }

        /// <inheritdoc />
        public IOvermock<T> ToBeCalled()
        {
            _callable.Calls(c => { }, Times.Any);
            return _overmock;
        }

        /// <inheritdoc />
        public IOvermock<T> ToBeCalled(Times times)
        {
            _callable.Calls(c => { }, times);
            return _overmock;
        }
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
    internal class SetupOvermock<T, TReturn> : SetupOvermock<T>, ISetup<T, TReturn> where T : class
    {
        private readonly IReturnable<TReturn> _returnable;
        private readonly IOvermock<T> _overmock;

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupOvermock{T, TReturn}"/> class.
        /// </summary>
        /// <param name="overmock"></param>
        /// <param name="returnable">The returnable.</param>
        internal SetupOvermock(IOvermock<T> overmock, IReturnable<TReturn> returnable) : base(overmock, returnable)
        {
            _overmock = overmock;
            _returnable = returnable;
        }

        /// <inheritdoc />
        public IOvermock<T> ToCall(Func<OvermockContext, TReturn> callback)
        {
            _returnable.Calls(callback, Times.Any);
            return _overmock;
        }

        /// <inheritdoc />
        public IOvermock<T> ToCall(Func<OvermockContext, TReturn> callback, Times times)
        {
            _returnable.Calls(callback, times);
            return _overmock;
        }

        /// <inheritdoc />
        public IOvermock ToReturn(TReturn result)
        {
            _returnable.Returns(result);
            return _overmock;
        }

        /// <summary>
        /// Converts to return.
        /// </summary>
        /// <param name="returnProvider">The return provider.</param>
        public IOvermock ToReturn(Func<TReturn> returnProvider)
        {
            _returnable.Returns(returnProvider);
            return _overmock;
        }

        IOvermock<T> ISetup<T>.ToCall(Action<OvermockContext> action)
        {
            return (IOvermock<T>)ToCall(action, Times.Any);
        }

        IOvermock<T> ISetup<T>.ToCall(Action<OvermockContext> action, Times times)
        {
            return (IOvermock<T>)ToCall(action, times);
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
