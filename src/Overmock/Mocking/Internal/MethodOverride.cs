
namespace Overmock.Mocking.Internal
{
	internal class MethodOverride : MemberOverride, IOverride
    {
        public MethodOverride(Delegate? overmock = default, Func<object>? returnProvider = default, Exception? exception = null) : base(returnProvider, exception)
        {
            Overmock = overmock;
        }

        public Delegate? Overmock { get; }
    }
}