using Overmock.Setup;
using System.Linq.Expressions;

namespace Overmock
{
    public static partial class OvermockExtensions
    {
        public static ISetupOvermock<T> Override<T>(this IOvermock<T> overmock, Expression<Action<T>> expression) where T : class
        {
            if (expression.Body is MethodCallExpression methodCall)
            {
                return new SetupOvermock<T>(
                    Overmocked.RegisterMethod(overmock, new MethodCall<T>(methodCall))
                );
            }

            throw new ArgumentException("Parameter must be a method call expression.");
        }

        public static ISetupOvermock<T, TResult> Override<T, TResult>(this IOvermock<T> overmock, Expression<Func<T, TResult>> expression) where T : class
        {
            if (expression.Body is MethodCallExpression method)
            {
                return new SetupOverride<T, TResult>(
                    Overmocked.RegisterMethod(overmock, new MethodCall<T, TResult>(method))
                );
            }

            if (expression.Body is MemberExpression property)
            {
                return new SetupPropertyOvermock<T, TResult>(
                    Overmocked.RegisterProperty(overmock, new PropertyCall<TResult>(property))
                );
            }

            throw new ArgumentException("Parameter must be a method call expression.");
        }
    }
}
