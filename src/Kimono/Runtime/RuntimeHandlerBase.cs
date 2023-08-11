namespace Kimono
{
	/// <summary>
	/// Class RuntimeHandlerBase.
	/// Implements the <see cref="Kimono.IRuntimeHandler" />
	/// </summary>
	/// <seealso cref="Kimono.IRuntimeHandler" />
	public abstract class RuntimeHandlerBase : IRuntimeHandler
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="RuntimeHandlerBase"/> class.
		/// </summary>
		/// <param name="runtimeContext">The runtime context.</param>
		protected RuntimeHandlerBase(RuntimeContext runtimeContext)
        {
            Context = runtimeContext;
        }

		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>The context.</value>
		protected RuntimeContext Context { get; }

		/// <summary>
		/// Handles the specified proxy.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>RuntimeHandlerResult.</returns>
		public RuntimeHandlerResult Handle(IProxy proxy, params object[] parameters)
        {
            return HandleCore(proxy, parameters);
        }

		/// <summary>
		/// Handles the core.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>RuntimeHandlerResult.</returns>
		protected abstract RuntimeHandlerResult HandleCore(IProxy proxy, params object[] parameters);
    }
}