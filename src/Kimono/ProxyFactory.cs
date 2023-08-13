using System.Reflection;
using System.Reflection.Emit;

namespace Kimono
{
	/// <summary>
	/// Provides a base implementation for <see cref="IProxyFactory"/>.
	/// Implements the <see cref="IProxyFactory" />
	/// </summary>
	/// <seealso cref="IProxyFactory" />
	public abstract class ProxyFactory : IProxyFactory
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="ProxyFactory"/> class.
		/// </summary>
		/// <param name="cache">The cache.</param>
		protected ProxyFactory(IProxyCache cache)
        {
			Cache = cache;
		}

		/// <summary>
		/// Gets the cache.
		/// </summary>
		/// <value>The cache.</value>
		protected IProxyCache Cache { get; }

		/// <summary>
		/// Creates the specified interceptor.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyGenerator&lt;T&gt;.</returns>
		public IProxyGenerator<T> Create<T>(IInterceptor<T> interceptor) where T : class
		{
			var generator = (IProxyGenerator<T>)Cache.GetGenerator(interceptor.TargetType)!;

			if (generator == null)
			{
				var context = CreateContext(interceptor);
				generator = CreateCore<T>(context);
			}

			return generator;
		}

		/// <summary>
		/// Creates the context.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>IProxyBuilderContext.</returns>
		protected abstract IProxyBuilderContext CreateContext(IInterceptor interceptor);

		/// <summary>
		/// Creates the core.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="marshallerContext">The marshaller context.</param>
		/// <returns>IProxyGenerator&lt;T&gt;.</returns>
		protected abstract IProxyGenerator<T> CreateCore<T>(IProxyBuilderContext marshallerContext) where T : class;

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>System.String.</returns>
		protected static string GetName(string name) => Constants.AssemblyAndTypeNameFormat.ApplyFormat(name);

		/// <summary>
		/// Gets the name of the assembly.
		/// </summary>
		/// <param name="name">The name.</param>
		/// <returns>AssemblyName.</returns>
		protected static AssemblyName GetAssemblyName(string name) => new AssemblyName(Constants.AssemblyDllNameFormat.ApplyFormat(GetName(name)));

		/// <summary>
		/// Interface IProxyBuilderContext
		/// </summary>
		protected interface IProxyBuilderContext
        {
			/// <summary>
			/// Gets the interceptor.
			/// </summary>
			/// <value>The interceptor.</value>
			IInterceptor Interceptor { get; }

			/// <summary>
			/// Gets the type builder.
			/// </summary>
			/// <value>The type builder.</value>
			TypeBuilder TypeBuilder { get; }

			/// <summary>
			/// Gets the proxy context.
			/// </summary>
			/// <value>The proxy context.</value>
			ProxyContext ProxyContext { get; }
        }
    }
}