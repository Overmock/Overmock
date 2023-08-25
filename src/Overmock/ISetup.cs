using System;

namespace Overmock
{
    /// <summary>
    /// Represents a member that can setup an Exception to throw when calling the overmock.
    /// </summary>
    public interface ISetup : IFluentInterface
    {
        /// <summary>
        /// Converts to becalled.
        /// </summary>
        void ToBeCalled();

        /// <summary>
        /// Converts to becalled.
        /// </summary>
        /// <param name="times">The times.</param>
        void ToBeCalled(Times times);

        /// <summary>
        /// Specifies the exception to throw when the overmocked member is called.
        /// </summary>
        /// <param name="exception">The exception to throw.</param>
        void ToThrow(Exception exception);
    }

    /// <summary>
    /// Interface ISetup
    /// Extends the <see cref="ISetup" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ISetup" />
    public interface ISetup<T> : ISetup where T : class
    {
        /// <summary>
        /// The current item being mocked.
        /// </summary>
        IOvermock<T> Overmock { get; }

        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="action">The action.</param>
        void ToCall(Action<OvermockContext> action);

        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="times">The times.</param>
        void ToCall(Action<OvermockContext> action, Times times);
    }

    /// <summary>
    /// Interface ISetup
    /// Extends the <see cref="ISetup{T}" />
    /// Extends the <see cref="ISetupReturn{TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="ISetup{T}" />
    /// <seealso cref="ISetupReturn{TReturn}" />
    public interface ISetup<T, TReturn> : ISetup<T>, ISetupReturn<TReturn> where T : class
    {
        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="callback">The callback.</param>
        void ToCall(Func<OvermockContext, TReturn> callback);

        /// <summary>
        /// Converts to call.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="times">The times.</param>
        void ToCall(Func<OvermockContext, TReturn> callback, Times times);
    }

    /// <summary>
    /// Interface ISetupMocks{T}
    /// Extends the <see cref="ISetup{T}" />
    /// Extends the <see cref="ISetupReturn{TReturn}" />
    /// Extends the <see cref="ISetupReturnMocks{TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="ISetup{T}" />
    /// <seealso cref="ISetupReturn{TReturn}" />
    /// <seealso cref="ISetupReturnMocks{TReturn}" />
    public interface ISetupMocks<T, TReturn> : ISetup<T, TReturn>, ISetupReturnMocks<TReturn> where T : class where TReturn : class
    {
    }
}
