using System.Linq.Expressions;

namespace Overmock
{
    public static partial class OvermockExtensions
    {
        public static IOverride<T> Override<T>(this IOvermock<T> overmock, Expression<Action<T>> expression) where T : class
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                return new Override<T>(
                    Overmocked.Register(overmock, new MethodCall<T>(methodCall))
                );
            }

            throw new ArgumentException("Parameter must be a method call expression.");
        }

        public static IOverride<T, TResult> Override<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                return new Override<T, TResult>(
                    Overmocked.Register(overmock, new MethodCall<T, TResult>(methodCall))
                );
            }

            throw new ArgumentException("Parameter must be a method call expression.");
        }
    }
}
