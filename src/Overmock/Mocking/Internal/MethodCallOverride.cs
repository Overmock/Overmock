
using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	internal class MethodCallOverride : IOverride
	{
		public MethodCallOverride(Delegate overmock)
		{
			Overmock = overmock;
		}

		public Delegate Overmock { get; }

		public object? Handle(RuntimeContext context)
		{
			return Overmock.DynamicInvoke(context);
		}
	}
}