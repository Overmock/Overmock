namespace Overmock
{
	/// <summary>
	/// Interface IExpectAnyInvocation
	/// </summary>
	public interface IExpectAnyInvocation
	{
		/// <summary>
		/// Expects any.
		/// </summary>
		/// <param name="value">if set to <c>true</c> [value].</param>
		void ExpectAny(bool value = true);
	}
}