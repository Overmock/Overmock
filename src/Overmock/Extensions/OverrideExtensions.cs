using System.Linq.Expressions;

namespace Overmock
{

	/// <summary>Contains extension methods used to override mocked members</summary>
	public static partial class OvermockExtensions
	{
		/// <summary>
		/// Overrides the specified method represented by an Action{T}.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Parameter must be a method call expression.</exception>
		public static ISetupOvermock<T> Override<T>(this IOvermock<T> overmock, Expression<Action<T>> expression) where T : class
		{
			if (expression.Body is MethodCallExpression methodCall)
			{
				return new SetupMethodOvermock<T>(
					Overmocked.RegisterMethod(overmock, new MethodCall<T>(methodCall))
				);
			}

			throw new ArgumentException("Parameter must be a method call expression.");
		}

		/// <summary>
		/// Overrides the specified method represented by an Func{T,TResult}.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TResult">The type of the result.</typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="expression">The expression.</param>
		/// <returns></returns>
		/// <exception cref="System.ArgumentException">Parameter must be a method call expression.</exception>
		public static ISetupOvermock<T, TResult> Override<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class
		{
			if (expression.Body is MethodCallExpression method)
			{
				return new SetupMethodOvermock<T, TResult>(
					Overmocked.RegisterMethod(overmock, new MethodCall<T, TResult>(method))
				);
			}

			if (expression.Body is MemberExpression property)
			{
				return new SetupPropertyOvermock<T, TResult>(
					Overmocked.RegisterProperty(overmock, new PropertyCall<TResult>(property))
				);
			}

			throw new ArgumentException("Parameter must be a method or property call expression.");
		}
	}
}
