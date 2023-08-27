using System.Linq.Expressions;
using System.Reflection;

namespace Overmocked.Mocking
{
    /// <summary>
    /// Interface IMethodCall
    /// Extends the <see cref="ICallable" />
    /// </summary>
    /// <seealso cref="ICallable" />
    public interface IMethodCall : ICallable
    {
        /// <summary>
        /// The <see cref="System.Linq.Expressions.Expression" /> used to select this method.
        /// </summary>
        /// <value>The expression.</value>
        MethodCallExpression Expression { get; }

        /// <summary>
        /// Gets the base method.
        /// </summary>
        /// <value>The base method.</value>
        MethodInfo BaseMethod { get; }

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>The method.</value>
        MethodInfo Method { get; }
    }
    /// <summary>
    /// Represents an overridden method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IMethodCall<T> : ICallable<T>, IMethodCall
    {
    }

    /// <summary>
    /// Represents an overridden method.
    /// </summary>
    /// <typeparam name="T">The owner type of this method.</typeparam>
    /// <typeparam name="TReturn">The return type of this method.</typeparam>
    public interface IMethodCall<T, TReturn> : IMethodCall<T>, ICallable<T>, IReturnable<TReturn> where T : class
    {
    }
}