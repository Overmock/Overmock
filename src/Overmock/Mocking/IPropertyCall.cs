using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking
{
	/// <summary>
	/// 
	/// </summary>
	public interface IPropertyCall : ICallable
	{
		/// <summary>
		/// The <see cref="System.Linq.Expressions.Expression"/> used to select this member.
		/// </summary>
		MemberExpression Expression { get; }

		/// <summary>
		/// 
		/// </summary>
		PropertyInfo PropertyInfo { get; }
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IPropertyCall<T, TReturn> : IPropertyCall, IReturnable<TReturn>
	{
	}
}