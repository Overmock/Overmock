using Kimono;

namespace Overmock.Benchmarks.Interceptors
{
    public class KimonoInvocationHandler : IInvocationHandler
    {
        public void Handle(IInvocationContext context)
        {
            context.Invoke();
        }
    }
}
