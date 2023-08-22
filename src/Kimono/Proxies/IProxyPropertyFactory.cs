﻿using System.Collections.Generic;
using System.Reflection;

namespace Kimono.Proxies
{
	/// <summary>
	/// Interface IProxyPropertyGenerator
	/// </summary>
	public interface IProxyPropertyFactory
    {
		/// <summary>
		/// Generates the specified context.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <param name="methods">The methods.</param>
		void Generate(IProxyContextBuilder context, IEnumerable<PropertyInfo> methods);

	}
}