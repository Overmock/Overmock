namespace Overmock.Mocking
{
	/// <summary>
	/// 
	/// </summary>
	public interface IThrowable : IOverridable
	{
		/// <summary>
		/// Throws the exception when called.
		/// </summary>
		/// <param name="exception"></param>
		void Throws(Exception exception);
	}
}