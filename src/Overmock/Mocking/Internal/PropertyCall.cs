using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// Class PropertyCall.
	/// Implements the <see cref="Overmock.Mocking.Internal.Returnable{T, TReturn}" />
	/// Implements the <see cref="Overmock.Mocking.IPropertyCall{T, TReturn}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TReturn">The type of the t return.</typeparam>
	/// <seealso cref="Overmock.Mocking.Internal.Returnable{T, TReturn}" />
	/// <seealso cref="Overmock.Mocking.IPropertyCall{T, TReturn}" />
	internal sealed class PropertyCall<T, TReturn> : Returnable<T, TReturn>, IPropertyCall<T, TReturn>
	{
		/// <summary>
		/// The expression
		/// </summary>
		private readonly MemberExpression _expression;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyCall{T, TReturn}"/> class.
		/// </summary>
		/// <param name="expression">The expression.</param>
		internal PropertyCall(MemberExpression expression) : base(expression.Member.Name)
		{
			_expression = expression;
		}

		/// <summary>
		/// Gets the expression.
		/// </summary>
		/// <value>The expression.</value>
		MemberExpression IPropertyCall.Expression => _expression;

		/// <summary>
		/// Gets the property information.
		/// </summary>
		/// <value>The property information.</value>
		PropertyInfo IPropertyCall.PropertyInfo => (PropertyInfo)_expression.Member;

		/// <summary>
		/// Gets the target.
		/// </summary>
		/// <returns>MemberInfo.</returns>
		public override MemberInfo GetTarget()
		{
			return _expression.Member;
		}
	}
}