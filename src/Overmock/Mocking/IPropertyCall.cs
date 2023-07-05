using Overmock.Runtime;
using System.Linq.Expressions;

namespace Overmock.Mocking
{
	/// <summary>
	/// Represents an overridden property.
	/// </summary>
	public interface IPropertyCall : IMemberCall
	{
		/// <summary>
		/// The <see cref="System.Linq.Expressions.Expression"/> used to select this member.
		/// </summary>
		MemberExpression Expression { get; }
	}

	/// <summary>
	/// Represents an overridden property.
	/// </summary>
	/// <typeparam name="TReturn">The property's return type.</typeparam>
	public interface IPropertyCall<TReturn> : IPropertyCall
	{
		/// <summary>
		/// An <see cref="Func{OverrideContext, TReturn}"/> delegate to call in place of this override's property.
		/// </summary>
		/// <param name="func"></param>
		void Calls(Func<RuntimeContext, TReturn> func);
	}
}