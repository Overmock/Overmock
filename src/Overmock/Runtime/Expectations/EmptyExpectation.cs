namespace Overmock.Runtime.Expectations
{
	/// <summary>
	/// An empty expectation.
	/// </summary>
	public class EmptyExpectation : Expectation
	{
		/// <inheritdoc/>
		public override void Assert()
		{
            // Noop
		}

		/// <inheritdoc/>
		public override void Returned(object? value = null)
		{
		}

		/// <inheritdoc/>
		protected override void WasCalledWithCore(params object[] parameters)
		{
		}
	}
}