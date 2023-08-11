using System.Reflection;

namespace Kimono
{
	internal class UnregisteredMemberRuntimeHandler : IRuntimeHandler
	{
		private readonly MethodInfo _method;

		public UnregisteredMemberRuntimeHandler(MethodInfo method)
		{
			_method = method;
		}

		public RuntimeHandlerResult Handle(IProxy proxy, params object[] parameters)
		{
			throw new KimonoException($"No override specified for the given method: {_method}");
		}
	}
}
