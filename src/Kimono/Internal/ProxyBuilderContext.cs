using System.Reflection.Emit;
using Kimono.Proxies;

namespace Kimono.Internal
{
	/// <summary>
	/// Class ProxyBuilderContext.
	/// Implements the <see cref="IProxyBuilderContext" />
	/// </summary>
	/// <seealso cref="IProxyBuilderContext" />
	public sealed class ProxyBuilderContext : IProxyBuilderContext
	{
		/// <summary>
		/// The method counter
		/// </summary>
		private int _methodCounter;

		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyBuilderContext"/> class.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <param name="typeBuilder">The type builder.</param>
		/// <param name="proxyType">Type of the proxy.</param>
		public ProxyBuilderContext(IInterceptor target, TypeBuilder typeBuilder, Type proxyType)
		{
			Interceptor = target;
			ProxyType = proxyType;
			TypeBuilder = typeBuilder;
			Interfaces = new List<Type>();
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
		/// Gets or sets the interfaces.
		/// </summary>
		/// <value>The interfaces.</value>
		private List<Type> Interfaces { get; set; }

		/// <summary>
		/// Adds the interfaces.
		/// </summary>
		/// <param name="interfaceTypes">The interface types.</param>
		public void AddInterfaces(params Type[] interfaceTypes)
		{
			var interfaces = Interfaces.ToArray();

			Interfaces = interfaces.Union(interfaceTypes).Distinct().ToList();
		}

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