
namespace Overmocked.Mocking.Internal
{
    /// <summary>
    /// Class ValueOverride.
    /// Implements the <see cref="IOverride" />
    /// </summary>
    /// <seealso cref="IOverride" />
    internal sealed class ValueOverride : Override, IOverride
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

        public override void Verify()
        {
        }

        protected override object? HandleCore(OvermockContext context)
        {
            return Value;
        }
    }
}