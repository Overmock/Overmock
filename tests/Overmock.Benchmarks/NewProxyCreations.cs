using Castle.DynamicProxy;
using Kimono;
using Overmocked.Benchmarks.Interceptors;
using Overmocked.Benchmarks.Models;
using System.Reflection;
using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Overmocked.Benchmarks
{
    public class NewProxyCreations : INewProxyCreations
    {
        private static readonly Benchmark _benchmarkClass = new Benchmark();

        //public void NewKimono(int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        var kimonoProxy = Intercept.WithHandler<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());
        //    }
        //}

        public void NewDotnet(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();
            }
        }

        public void NewCastle(int count)
        {
            var generator = new ProxyGenerator();

            for (int i = 0; i < count; i++)
            {
                var castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, new CastleInterceptor());
            }
        }

        public void NewKimonoCore(int count)
        {
            var interceptor = new Interceptor<IBenchmark>();
            var factory = ProxyFactory.Create();

            for (int i = 0; i < count; i++)
            {
                var kimonoProxy = factory.CreateInterfaceProxy(interceptor);
            }
        }
    }
}
