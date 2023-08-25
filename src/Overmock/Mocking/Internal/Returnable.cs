using System;
using System.Collections.Generic;

namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// Class Returnable.
    /// Implements the <see cref="Overmock.Mocking.Internal.Callable" />
    /// Implements the <see cref="Overmock.Mocking.IReturnable{TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="Overmock.Mocking.Internal.Callable" />
    /// <seealso cref="Overmock.Mocking.IReturnable{TReturn}" />
    internal abstract class Returnable<T, TReturn> : Callable, IReturnable<TReturn>
    {
        protected Returnable(string name) : base(name)
        {
        }

        /// <summary>
        /// Calls the specified function.
        /// </summary>
        /// <param name="func">The function.</param>
        /// <param name="times">The times.</param>
        public void Calls(Func<OvermockContext, TReturn> func, Times times)
        {
            Override = new MethodCallOverride(times, c => func(c));
        }

        /// <summary>
        /// Returns the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Returns(TReturn value)
        {
            Returns(() => value);
        }

        /// <summary>
        /// Returns the specified function.
        /// </summary>
        /// <param name="func">The function.</param>
        public void Returns(Func<TReturn> func)
        {
            Override = new MethodCallOverride(Times.Any, new Func<OvermockContext, object?>(c => func()));
        }

        protected override void Verify()
        {
            var overrides = GetOverrides();

            foreach (var item in overrides)
            {
                item.Verify();
            }
        }

        /// <inheritdoc />
        protected override void AddOverridesTo(List<IOverride> overrides)
        {
            if (Override != null)
            {
                overrides.Add(Override);
            }

            base.AddOverridesTo(overrides);
        }
    }
}