using Overmock.Mocking.Internal;
using System;
using System.Linq.Expressions;

namespace Overmock
{
	/// <summary>
	/// Contains extension methods used to override mocked members
	/// </summary>
	public static partial class OvermockExtensions
	{
		/// <summary>
		/// Overrides the specified method represented by an Action{T}.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="overmock">The overmock.</param>
		/// <param name="expression">The expression.</param>
		/// <returns>ISetup&lt;T&gt;.</returns>
		/// <exception cref="System.ArgumentException">Parameter must be a method call expression.</exception>
		public static ISetup<T> Mock<T>(this IOvermock<T> overmock, Expression<Action<T>> expression) where T : class
		{
			if (expression.Body is MethodCallExpression methodCall)
			{
				return new SetupOvermock<T>(
					Over.RegisterMethod(overmock, new MethodCall<T>(methodCall))
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
		/// <returns>ISetup&lt;T, TResult&gt;.</returns>
		/// <exception cref="System.ArgumentException">Parameter must be a method or property call expression.</exception>
		public static ISetup<T, TResult> Mock<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class
		{
			if (expression.Body is MethodCallExpression method)
			{
				return new SetupOvermock<T, TResult>(
					Over.RegisterMethod(overmock, new MethodCall<T, TResult>(method))
				);
			}

			if (expression.Body is MemberExpression property)
			{
				return new SetupOvermock<T, TResult>(
					Over.RegisterProperty(overmock, new PropertyCall<T, TResult>(property))
				);
			}

			throw new ArgumentException("Parameter must be a method or property call expression.");
        }

        /// <summary>
        /// Overrides the specified method represented by an Func{T,TResult}.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="overmock">The overmock.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>ISetup&lt;T, TResult&gt;.</returns>
        /// <exception cref="System.ArgumentException">Parameter must be a method or property call expression.</exception>
        public static ISetupMocks<T, TResult> Overmock<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class where TResult : class
        {
            if (expression.Body is MethodCallExpression method)
            {
                return new SetupOvermockWithMockReturns<T, TResult>(
                    Over.RegisterMethod(overmock, new MethodCall<T, TResult>(method))
                );
            }

            if (expression.Body is MemberExpression property)
            {
                return new SetupOvermockWithMockReturns<T, TResult>(
                    Over.RegisterProperty(overmock, new PropertyCall<T, TResult>(property))
                );
            }

            throw new ArgumentException("Parameter must be a method or property call expression.");
        }
    }
}
