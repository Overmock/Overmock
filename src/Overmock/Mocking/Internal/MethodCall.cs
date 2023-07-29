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

		public override object? GetDefaultReturnValue()
		{
			return null;
		}

		public override MemberInfo GetTarget()
		{
			throw new NotImplementedException();
		}
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

		public override object? GetDefaultReturnValue()
		{
			return default(TReturn);
		}

		public override MemberInfo GetTarget()
		{
			return _expression.Method;
		}
	}
}