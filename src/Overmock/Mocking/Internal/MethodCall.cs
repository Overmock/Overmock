using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking.Internal
{
    /// <summary>
    /// Class MethodCall.
    /// Implements the <see cref="Callable{T}" />
    /// Implements the <see cref="IMethodCall{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Callable{T}" />
    /// <seealso cref="IMethodCall{T}" />
    internal sealed class MethodCall<T> : Callable<T>, IMethodCall<T> where T : class
    {
        /// <summary>
        /// The expression
        /// </summary>
        private readonly MethodCallExpression _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCall{T}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        internal MethodCall(MethodCallExpression expression) : base(expression.Method.Name)
        {
            _expression = expression;
            BaseMethod = _expression.Method.IsGenericMethod
                ? _expression.Method.GetGenericMethodDefinition()
                : _expression.Method;
        }

        /// <summary>
        /// Gets the base method.
        /// </summary>
        /// <value>The base method.</value>
        public MethodInfo BaseMethod { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method => _expression.Method;

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>The expression.</value>
        internal MethodCallExpression Expression => _expression;

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>The expression.</value>
        MethodCallExpression IMethodCall.Expression => Expression;

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns>MemberInfo.</returns>
        public override MemberInfo GetTarget()
        {
            return Method;
        }
    }

    /// <summary>
    /// Class MethodCall.
    /// Implements the <see cref="Returnable{T, TReturn}" />
    /// Implements the <see cref="IMethodCall{T, TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="Returnable{T, TReturn}" />
    /// <seealso cref="IMethodCall{T, TReturn}" />
    internal sealed class MethodCall<T, TReturn> : Returnable<T, TReturn>, IMethodCall<T, TReturn> where T : class
    {
        /// <summary>
        /// The expression
        /// </summary>
        private readonly MethodCallExpression _expression;

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodCall{T, TReturn}"/> class.
        /// </summary>
        /// <param name="expression">The expression.</param>
        internal MethodCall(MethodCallExpression expression) : base(expression.Method.Name)
        {
            _expression = expression;
            BaseMethod = _expression.Method.IsGenericMethod
                ? _expression.Method.GetGenericMethodDefinition()
                : _expression.Method;
        }

        /// <summary>
        /// Gets the base method.
        /// </summary>
        /// <value>The base method.</value>
        public MethodInfo BaseMethod { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        public MethodInfo Method => _expression.Method;

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>The expression.</value>
        internal MethodCallExpression Expression => _expression;

        /// <summary>
        /// Gets the expression.
        /// </summary>
        /// <value>The expression.</value>
        MethodCallExpression IMethodCall.Expression => Expression;

        /// <summary>
        /// Gets the target.
        /// </summary>
        /// <returns>MemberInfo.</returns>
        public override MemberInfo GetTarget()
        {
            return _expression.Method;
        }
    }
}