using System;
using System.Collections.Generic;

namespace Overmock.Mocking.Internal
{

    /// <summary>
    /// Class Throwable.
    /// Implements the <see cref="Overridable" />
    /// Implements the <see cref="IThrowable" />
    /// </summary>
    /// <seealso cref="Overridable" />
    /// <seealso cref="IThrowable" />
    internal abstract class Throwable : Overridable, IThrowable
    {
        protected Throwable(string name) : base(name)
        {
        }

        public IOverride? Exception { get; private set; }

        /// <inheritdoc />
        public void Throws(Exception exception)
        {
            Exception = new ThrowExceptionOverride(exception);
        }

        /// <inheritdoc />
        protected override void AddOverridesTo(List<IOverride> overrides)
        {
            if (Exception != null)
            {
                overrides.Add(Exception);
            }
        }
    }
}