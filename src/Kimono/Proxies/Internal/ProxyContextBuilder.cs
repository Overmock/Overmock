using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Kimono.Proxies;

namespace Kimono.Internal
{
	/// <summary>
	/// Class ProxyBuilderContext.
	/// Implements the <see cref="IProxyContextBuilder" />
	/// </summary>
	/// <seealso cref="IProxyContextBuilder" />
	public sealed class ProxyContextBuilder : IProxyContextBuilder
    {
		private int _methodCounter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyContextBuilder"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="typeBuilder">The type builder.</param>
		/// <param name="proxyType">Type of the proxy.</param>
		public ProxyContextBuilder(IInterceptor target, TypeBuilder typeBuilder, Type proxyType)
		{
			Interceptor = target;
			ProxyType = proxyType;
			TypeBuilder = typeBuilder;
			ProxyContext = new ProxyContext();
		}

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		public IInterceptor Interceptor { get; }

		/// <summary>
		/// Gets the type of the proxy.
		/// </summary>
		/// <value>The type of the proxy.</value>
		public Type ProxyType { get; }

		/// <summary>
		/// Gets the type builder.
		/// </summary>
		/// <value>The type builder.</value>
		public TypeBuilder TypeBuilder { get; }

		/// <summary>
		/// Gets the proxy context.
		/// </summary>
		/// <value>The proxy context.</value>
		public ProxyContext ProxyContext { get; }

		/// <summary>
		/// Gets the next method identifier.
		/// </summary>
		/// <returns>System.Int32.</returns>
		public int GetNextMethodId()
		{
			return _methodCounter++;
		}
	}
}