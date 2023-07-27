namespace Overmock.Runtime.Expectations
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Expectation : IExpectation
	{
		/// <inheritdoc/>
		public uint CallCount { get; protected set; }

		/// <inheritdoc/>
		public abstract void Assert();

		/// <inheritdoc/>
		public abstract void Returned(object? value = null);

		/// <summary>
		/// 
		/// </summary>
		public virtual void WasCalledWith(params object[] parameters)
		{
			IncrementCallCount();

			WasCalledWithCore(parameters);
		}

		/// <inheritdoc/>
		protected abstract void WasCalledWithCore(params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="times"></param>
		protected void IncrementCallCount(uint times = 1)
		{
			CallCount += times;
		}
	}
}