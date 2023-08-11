using System.Reflection;

namespace Kimono
{
	/// <summary>
	/// Class UnregisteredMemberRuntimeHandler.
	/// Implements the <see cref="Kimono.IRuntimeHandler" />
	/// </summary>
	/// <seealso cref="Kimono.IRuntimeHandler" />
	internal class UnregisteredMemberRuntimeHandler : IRuntimeHandler
	{
		/// <summary>
		/// The method
		/// </summary>
		private readonly MethodInfo _method;

		/// <summary>
		/// Initializes a new instance of the <see cref="UnregisteredMemberRuntimeHandler"/> class.
		/// </summary>
		/// <param name="method">The method.</param>
		public UnregisteredMemberRuntimeHandler(MethodInfo method)
		{
			_method = method;
		}

		/// <summary>
		/// Handles the specified proxy.
		/// </summary>
		/// <param name="proxy">The proxy.</param>
		/// <param name="parameters">The parameters.</param>
		/// <returns>RuntimeHandlerResult.</returns>
		/// <exception cref="Kimono.KimonoException">No override specified for the given method: {_method}</exception>
		public RuntimeHandlerResult Handle(IProxy proxy, params object[] parameters)
		{
			throw new KimonoException($"No override specified for the given method: {_method}");
		}
	}
}
