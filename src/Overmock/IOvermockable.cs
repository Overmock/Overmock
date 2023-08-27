using Overmocked.Mocking;
using System.ComponentModel;

namespace Overmocked
{
    /// <summary>
    /// Provides methods to add overmocked properties and methods.
    /// </summary>
    public interface IOvermockable
    {
        /// <summary>
        /// Adds the method.
        /// </summary>
        /// <typeparam name="TMethod">The type of the method.</typeparam>
        /// <param name="method">The method.</param>
        /// <returns>TMethod.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;

        /// <summary>
        /// Adds the property.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="propertyCall">The property.</param>
        /// <returns>TProperty.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        TProperty AddProperty<TProperty>(TProperty propertyCall) where TProperty : IPropertyCall;
    }
}