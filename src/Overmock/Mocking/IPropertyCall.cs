using System.Linq.Expressions;
using System.Reflection;

namespace Overmocked.Mocking
{
    /// <summary>
    /// Interface IPropertyCall
    /// Extends the <see cref="ICallable" />
    /// </summary>
    /// <seealso cref="ICallable" />
    public interface IPropertyCall : ICallable
    {
        /// <summary>
        /// The <see cref="System.Linq.Expressions.Expression" /> used to select this member.
        /// </summary>
        /// <value>The expression.</value>
        MemberExpression Expression { get; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        /// <value>The property information.</value>
        PropertyInfo PropertyInfo { get; }
    }

    /// <summary>
    /// Interface IPropertyCall
    /// Extends the <see cref="IPropertyCall" />
    /// Extends the <see cref="IReturnable{TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="IPropertyCall" />
    /// <seealso cref="IReturnable{TReturn}" />
    public interface IPropertyCall<T, TReturn> : IPropertyCall, IReturnable<TReturn>
    {
    }
}