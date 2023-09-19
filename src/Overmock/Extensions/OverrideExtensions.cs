using Overmocked.Mocking.Internal;
using System;
using System.Linq.Expressions;

namespace Overmocked
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
            var overmockable = GetOvermockableOrThrow(overmock);

            if (expression.Body is MethodCallExpression method)
            {
                var methodCall = Overmock.RegisterMethod<T>(overmockable, method);
                return new SetupOvermock<T>(overmock, methodCall);
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
            var overmockable = GetOvermockableOrThrow(overmock);

            if (expression.Body is MethodCallExpression method)
            {
                var methodCall = Overmock.RegisterMethod<T, TResult>(overmockable, method);
                return new SetupOvermock<T, TResult>(overmock, methodCall);
            }

            if (expression.Body is MemberExpression property)
            {
                var propertyCall = Overmock.RegisterProperty(
                    overmockable,
                    new PropertyCall<T, TResult>(property)
                );
                return new SetupOvermock<T, TResult>(overmock, propertyCall);
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
        public static ISetupMocks<T, TResult> OverMock<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class where TResult : class
        {
            var overmockable = GetOvermockableOrThrow(overmock);

            if (expression.Body is MethodCallExpression method)
            {
                var methodCall = Overmock.RegisterMethod<T, TResult>(overmockable, method);
                return new SetupOvermockWithMockReturns<T, TResult>(overmock, methodCall);
            }

            if (expression.Body is MemberExpression property)
            {
                var propertyCall = Overmock.RegisterProperty(
                    overmockable,
                    new PropertyCall<T, TResult>(property)
                );
                return new SetupOvermockWithMockReturns<T, TResult>(overmock, propertyCall);
            }

            throw new ArgumentException("Parameter must be a method or property call expression.");
        }
        
        private static IOvermockable GetOvermockableOrThrow<T>(IOvermock<T> overmock) where T : class
        {
            if (overmock is IOvermockable overmockable)
            {
                return overmockable;
            }

            throw new OvermockException($"Cannot overmock a type that is not {nameof(IOvermockable)}");
        }
    }
}
