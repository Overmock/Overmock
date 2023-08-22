
using System;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class MethodCallOverride.
	/// Implements the <see cref="Overmock.Mocking.IOverride" />
	/// </summary>
	/// <seealso cref="Overmock.Mocking.IOverride" />
	internal sealed class MethodCallOverride : Override
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCallOverride"/> class.
        /// </summary>
        /// <param name="times">The times.</param>
        /// <param name="overmock">The overmock.</param>
        public MethodCallOverride(Times times, Func<OvermockContext, object?> overmock)
        {
			Times = times;
            Overmock = overmock;
        }

		/// <summary>
		/// Gets the overmock.
		/// </summary>
		/// <value>The overmock.</value>
		public Delegate Overmock { get; }

		/// <summary>
		/// Gets the times.
		/// </summary>
		/// <value>The times.</value>
		public Times Times { get; }

        public override void Verify()
        {
            Times.ThrowIfInvalid(TimesCalled);
        }

        protected override object? HandleCore(OvermockContext context)
        {
            Times.ThrowIfInvalid(TimesCalled);
            return Overmock.DynamicInvoke(context);
        }
    }
}