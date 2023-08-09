
namespace Overmock.Mocking
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IReturnable<T> : ICallable
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		void Returns(T value);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		void Returns(Func<T> func);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="func"></param>
		/// <param name="times"></param>
		void Calls(Func<OvermockContext, T> func, Times times);
	}
}