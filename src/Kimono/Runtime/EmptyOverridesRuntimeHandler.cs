using Kimono;

namespace Kimono.Runtime
{
	/// <summary>
	/// Class EmptyOverridesRuntimeHandler.
	/// Implements the <see cref="RuntimeHandlerBase" />
	/// </summary>
	/// <seealso cref="RuntimeHandlerBase" />
	public class EmptyOverridesRuntimeHandler : RuntimeHandlerBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EmptyOverridesRuntimeHandler"/> class.
		/// </summary>
		/// <param name="runtimeContext">The runtime context.</param>
		public EmptyOverridesRuntimeHandler(RuntimeContext runtimeContext) : base(runtimeContext)
		{
		}

		/// <summary>
		/// Handles the core.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>RuntimeHandlerResult.</returns>
		protected override RuntimeHandlerResult HandleCore(IProxy proxy, params object[] parameters)
		{
			return RuntimeHandlerResult.Empty;
		}
	}
}