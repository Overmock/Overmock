
using System;

namespace Overmock.Mocking
{
    /// <summary>
    /// Interface IReturnable
    /// Extends the <see cref="ICallable" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="ICallable" />
    public interface IReturnable<T> : ICallable
    {
        /// <summary>
        /// Returns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        void Returns(T value);

        /// <summary>
        /// Returns the specified function.
        /// </summary>
        /// <param name="func">The function.</param>
        void Returns(Func<T> func);

        /// <summary>
        /// Calls the specified function.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="times">The times.</param>
        void Calls(Func<OvermockContext, T> func, Times times);
    }
}