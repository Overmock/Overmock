using Overmock.Runtime;
using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking.Internal
{
	internal class MethodCall<T> : Callable<T>, IMethodCall<T> where T : class
	{
		private readonly MethodCallExpression _expression;

		internal MethodCall(MethodCallExpression expression)
		{
			_expression = expression;
		}

		public MethodInfo Method => _expression.Method;

		internal MethodCallExpression Expression => _expression;

		MethodCallExpression IMethodCall.Expression => Expression;
	}

	internal class MethodCall<T, TReturn> : Returnable<T, TReturn>, IMethodCall<T, TReturn> where T : class
	{
		private readonly MethodCallExpression _expression;

		internal MethodCall(MethodCallExpression expression)
		{
			_expression = expression;
		}

		public MethodInfo Method => _expression.Method;

		internal MethodCallExpression Expression => _expression;

		MethodCallExpression IMethodCall.Expression => Expression;
	}
}