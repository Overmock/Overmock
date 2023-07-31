using System.Reflection;

namespace Overmock.Proxies
{
	internal class UnregisteredMemberRuntimeHandler : IRuntimeHandler
	{
		private readonly MethodInfo _method;

		public UnregisteredMemberRuntimeHandler(MethodInfo method)
		{
			_method = method;
		}

		public RuntimeHandlerResult Handle(params object[] parameters)
		{
			throw new OvermockException($"No override specified for the given method: {_method}");
		}
	}
}
