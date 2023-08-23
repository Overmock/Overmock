using System.Collections.Generic;
using System.Reflection;

namespace Kimono.Proxies
{
	/// <summary>
	/// Interface IProxyPropertyGenerator
	/// </summary>
	public interface IProxyPropertyFactory
    {
        /// <summary>
        /// Creates the specified properties.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="properties">The properties.</param>
        void Create(IProxyContextBuilder context, IEnumerable<PropertyInfo> properties);

	}
}