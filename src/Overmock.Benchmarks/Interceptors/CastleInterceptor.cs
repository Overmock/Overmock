using Castle.DynamicProxy;

namespace Overmock.Benchmarks.Interceptors
{
    public class CastleInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
        }
    }
}
