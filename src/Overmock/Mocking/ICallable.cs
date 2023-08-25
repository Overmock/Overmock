using System;

namespace Overmock.Mocking
{
    /// <summary>
    /// Represents a member that is overridden
    /// </summary>
    public interface ICallable : IThrowable
    {
        /// <summary>
        /// An <see cref="Func{OverrideContext, TReturn}" /> delegate to call in place of this override's property.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="times">The times.</param>
        void Calls(Action<OvermockContext> action, Times times);
    }

    /// <summary>
    /// Interface ICallable
    /// Extends the <see cref="Overmock.Mocking.ICallable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Overmock.Mocking.ICallable" />
    public interface ICallable<T> : ICallable
    {
    }
}