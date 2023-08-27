using Castle.DynamicProxy;

namespace Overmocked.Benchmarks.Interceptors
{
    public class CastleInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
