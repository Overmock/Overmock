using System.Linq.Expressions;
using System.Reflection;

namespace Overmock.Mocking
{
    /// <summary>
    /// Interface IPropertyCall
    /// Extends the <see cref="Overmock.Mocking.ICallable" />
    /// </summary>
    /// <seealso cref="Overmock.Mocking.ICallable" />
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
    /// Extends the <see cref="Overmock.Mocking.IPropertyCall" />
    /// Extends the <see cref="Overmock.Mocking.IReturnable{TReturn}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    /// <seealso cref="Overmock.Mocking.IPropertyCall" />
    /// <seealso cref="Overmock.Mocking.IReturnable{TReturn}" />
    public interface IPropertyCall<T, TReturn> : IPropertyCall, IReturnable<TReturn>
    {
    }
}