
namespace Overmock.Mocking.Internal
{
	internal class MethodCallOverride : IOverride
	{
		private int _callCount;
		public MethodCallOverride(Delegate overmock, Times times)
		{
			Overmock = overmock;
			Times = times;
		}

		public Delegate Overmock { get; }

		public Times Times { get; }

		public object? Handle(OvermockContext context)
		{
			Times.ThrowIfInvalid(++_callCount);
			return Overmock.DynamicInvoke(context);
		}
	}
}