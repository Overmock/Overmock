
namespace Overmock.Mocking.Internal
{
    public class MemberOverride : ThrowExceptionOverride, IOverride
    {
        internal MemberOverride(Func<object>? returnProvider = default, Exception? exception = default) : base(exception)
        {
            ReturnProvider = returnProvider;
        }

        public Func<object>? ReturnProvider { get; }
    }

    public class MethodOverride : MemberOverride, IOverride
    {
        public MethodOverride(Delegate? overmock = default, Func<object>? returnProvider = default, Exception? exception = null) : base(returnProvider, exception)
        {
            Overmock = overmock;
        }

        public Delegate? Overmock { get; }
    }

    public class PropetyOverride : MemberOverride, IOverride
    {

    }
}