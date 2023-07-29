using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking
{
	/// <summary>
	/// 
	/// </summary>
	public interface IMethodCall : ICallable
	{
		/// <summary>
		/// The <see cref="System.Linq.Expressions.Expression"/> used to select this method.
		/// </summary>
		MethodCallExpression Expression { get; }

		/// <summary>
		/// 
		/// </summary>
		MethodInfo Method { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		object? GetDefaultReturnValue();
	}
	/// <summary>
	/// Represents an overridden method.
	/// </summary>
	public interface IMethodCall<T> : ICallable<T>, IMethodCall
	{
	}

	/// <summary>
	/// Represents an overridden method.
	/// </summary>
	/// <typeparam name="T">The owner type of this method.</typeparam>
	/// <typeparam name="TReturn">The return type of this method.</typeparam>
	public interface IMethodCall<T, TReturn> : IMethodCall<T>, ICallable<T>, IReturnable<TReturn> where T : class
	{
	}
}