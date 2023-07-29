using Overmock.Runtime;
using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking.Internal
{
	internal class PropertyCall<T, TReturn> : Returnable<T, TReturn>, IPropertyCall<T, TReturn>
	{
		private readonly MemberExpression _expression;

		internal PropertyCall(MemberExpression expression)
		{
			_expression = expression;
		}

		MemberExpression IPropertyCall.Expression => _expression;

		PropertyInfo IPropertyCall.PropertyInfo => (PropertyInfo)_expression.Member;
	}
}