using System.Reflection;

namespace Overmock.Runtime
{
	internal class UnregisteredOverrideHandler : IOverrideHandler
	{
		private readonly MethodInfo _method;

		public UnregisteredOverrideHandler(MethodInfo method)
		{
			_method = method;
		}

		public OverrideHandlerResult Handle(params object[] parameters)
		{
			throw new OvermockException($"No override specified for the given method: ");
		}
	}
}
