﻿
namespace Overmocked.Mocking.Internal
{
    /// <summary>
    /// Class ValueOverride.
    /// Implements the <see cref="IOverride" />
    /// </summary>
    /// <seealso cref="IOverride" />
    internal sealed class ValueOverride : Verifiable, IOverride
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueOverride" /> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public ValueOverride(object value)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public object Value { get; }

        /// <summary>
        /// Handles the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>System.Nullable&lt;System.Object&gt;.</returns>
        public object? Handle(OvermockContext context)
        {
            return Value;
        }

        protected override void Verify()
        {
        }
    }
}