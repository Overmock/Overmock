
using System;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class ReturnProviderOverride.
	/// Implements the <see cref="IOverride" />
	/// </summary>
	/// <seealso cref="IOverride" />
	public class ReturnProviderOverride : Verifiable, IOverride
	{
        private bool _calledProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReturnProviderOverride" /> class.
        /// </summary>
        /// <param name="returnProvider">The return provider.</param>
        internal ReturnProviderOverride(Func<object> returnProvider)
		{
			ReturnProvider = returnProvider;
		}

		/// <summary>
		/// Gets the return provider.
		/// </summary>
		/// <value>The return provider.</value>
		public Func<object> ReturnProvider { get; }

		/// <summary>
		/// Handles the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public object? Handle(OvermockContext context)
		{
            _calledProvider = true;

            return ReturnProvider();
		}

        /// <summary>
        /// 
        /// </summary>
        protected override void Verify()
        {
            if (!_calledProvider)
            {
                throw new VerifyException(this, "ReturnProvider has not been invoked.");
            }
        }
    }
}