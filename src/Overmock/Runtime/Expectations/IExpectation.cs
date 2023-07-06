namespace Overmock.Runtime.Expectations
{
	/// <summary>
	/// 
	/// </summary>
	public interface IExpectation
    {
		/// <summary>
		/// 
		/// </summary>
		uint CallCount { get; }
		
		/// <summary>
		/// 
		/// </summary>
		void WasCalledWith(params object[] parameters);

		/// <summary>
		/// 
		/// </summary>
		void Returned(object? value = null);

		/// <summary>
		/// 
		/// </summary>
		void Assert();
    }
}