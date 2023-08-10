
namespace Overmock.Mocking
{
	/// <summary>
	/// Represents a member that is overridden
	/// </summary>
	public interface ICallable : IThrowable
	{
		/// <summary>
		/// 
		/// </summary>
		Times Times { get; set; }

		/// <summary>
		/// An <see cref="Func{OverrideContext, TReturn}"/> delegate to call in place of this override's property.
		/// </summary>
		/// <param name="action"></param>
		/// <param name="times"></param>
		void Calls(Action<OvermockContext> action, Times times);


		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		object? GetDefaultReturnValue();
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICallable<T> : ICallable
	{
	}
}