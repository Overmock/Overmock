using Overmocked.Mocking;
using System.Collections.Generic;
using System.ComponentModel;

namespace Overmocked
{
    /// <summary>
    /// Provides the methods to get overmocked properties and methods.
    /// </summary>
    public interface IOvermocked
    {
        /// <summary>
        /// Gets the overmocked methods.
        /// </summary>
        /// <returns>IEnumerable&lt;IMethodCall&gt;.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IMethodCall> GetMethods();

        /// <summary>
        /// Gets the overmocked properties.
        /// </summary>
        /// <returns>IEnumerable&lt;IPropertyCall&gt;.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IPropertyCall> GetProperties();
    }
}