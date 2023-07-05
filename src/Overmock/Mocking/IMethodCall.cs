using Overmock.Runtime;
using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking
{
	/// <summary>
	/// Represents an overridden method.
	/// </summary>
	public interface IMethodCall : IMemberCall
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
		/// An <see cref="Action"/> delegate to call in place of this override's method.
		/// </summary>
		/// <param name="method">The method to call.</param>
		void Calls(Action<RuntimeContext> method);
	}

	/// <summary>
	/// Represents an overridden method.
	/// </summary>
	/// <typeparam name="T">The owner type of this method.</typeparam>
	public interface IMethodCall<T> : IMethodCall where T : class
	{
	}

	/// <summary>
	/// Represents an overridden method.
	/// </summary>
	/// <typeparam name="T">The owner type of this method.</typeparam>
	/// <typeparam name="TReturn">The return type of this method.</typeparam>
	public interface IMethodCall<T, TReturn> : IMethodCall<T> where T : class
	{
		/// <summary>
		/// An <see cref="Func{OverrideContext, TReturn}"/> delegate to call in place of this override's method.
		/// </summary>
		/// <param name="method">The method to call.</param>
		void Calls(Func<RuntimeContext, TReturn> method);
	}
}