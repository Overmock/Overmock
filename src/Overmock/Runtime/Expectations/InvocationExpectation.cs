namespace Overmock.Runtime.Expectations
{
	/// <summary>
	/// An empty expectation.
	/// </summary>
	public class InvocationExpectation : Expectation
	{
		/// <summary>
		/// 
		/// </summary>
		public InvocationExpectation(int expectedCallCount = -1)
		{
			ExpectedCallCount = expectedCallCount;
		}

		/// <summary>
		/// 
		/// </summary>
		public int ExpectedCallCount { get; }

		/// <inheritdoc/>
		protected override void WasCalledWithCore(params object[] parameters)
		{
		}

		/// <inheritdoc/>
		public override void Assert()
		{
			// Noop
		}

		/// <inheritdoc/>
		public override void Returned(object? value = null)
		{
		}
	}
}