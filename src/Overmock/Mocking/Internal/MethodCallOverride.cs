
namespace Overmock.Mocking.Internal
{
	internal class MethodCallOverride : IOverride
	{
		public MethodCallOverride(Delegate overmock)
		{
			Overmock = overmock;
		}

		public Delegate Overmock { get; }

		public object? Handle(OvermockContext context)
		{
			return Overmock.DynamicInvoke(context);
		}
	}
}