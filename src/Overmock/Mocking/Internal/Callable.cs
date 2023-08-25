
using System;
using System.Collections.Generic;

namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// Class Callable.
    /// Implements the <see cref="Throwable" />
    /// Implements the <see cref="ICallable" />
    /// </summary>
    /// <seealso cref="Throwable" />
    /// <seealso cref="ICallable" />
    internal abstract class Callable : Throwable, ICallable
    {
        protected Callable(string name) : base(name)
        {
        }

        /// <summary>
        /// Gets the action.
        /// </summary>
        /// <value>The action.</value>
        public IOverride? Override { get; protected set; }

        /// <inheritdoc />
        public void Calls(Action<OvermockContext> action, Times times)
        {
            Override = new MethodCallOverride(times, c => {
                action(c);
                return null;
            });
        }

        /// <summary>
        /// Adds the overrides to.
        /// </summary>
        /// <param name="overrides">The overrides.</param>
        protected override void AddOverridesTo(List<IOverride> overrides)
        {
            if (Override != null)
            {
                overrides.Add(Override);
            }

            base.AddOverridesTo(overrides);
        }
    }

    /// <summary>
    /// Class Callable.
    /// Implements the <see cref="Callable" />
    /// Implements the <see cref="ICallable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Callable" />
    /// <seealso cref="ICallable{T}" />
    internal abstract class Callable<T> : Callable, ICallable<T>
    {
        protected Callable(string name) : base(name)
        {
        }
    }
}