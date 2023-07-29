using Overmock.Runtime;

namespace Overmock.Mocking
{
	/// <summary>
	/// Represents a member that is overridden
	/// </summary>
	public interface ICallable : IThrowable
	{
		/// <summary>
		/// An <see cref="Func{OverrideContext, TReturn}"/> delegate to call in place of this override's property.
		/// </summary>
		/// <param name="action"></param>
		void Calls(Action<RuntimeContext> action);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICallable<T> : ICallable
	{
	}
}