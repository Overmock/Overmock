using Castle.DynamicProxy;
using Kimono;
using Overmocked.Benchmarks.Interceptors;
using Overmocked.Benchmarks.Models;
using System.Reflection;

namespace Overmocked.Benchmarks
{
    public class InterceptorMethodCalls : IInterceptorMethodCalls
    {
        private static readonly Benchmark _benchmarkClass = new Benchmark();
//        private static readonly IBenchmark _kimonoProxy;
        private static readonly IBenchmark _kimonoProxyCore;
        private static readonly IBenchmark _dispatchProxy;
        private static readonly IBenchmark _castleProxy;
        private static readonly List<string> _list = new List<string> { "world" };
        
        static InterceptorMethodCalls()
        {
 //           _kimonoProxy = Intercept.WithHandlers<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());

            var interceptor = new Interceptor<IBenchmark>();
            _kimonoProxyCore = ProxyFactory.Create().CreateInterfaceProxy(interceptor);

            _dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();

            var generator = new ProxyGenerator();
            var interceptors = new List<Castle.DynamicProxy.IInterceptor> { new CastleInterceptor() };
            _castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, interceptors.ToArray());
        }

        public InterceptorMethodCalls()
        {
        }

        public static void Castle(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _castleProxy.VoidWith3Params("hello", 20, _list);
            }
        }

        public static void Dotnet(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _dispatchProxy.VoidWith3Params("hello", 20, _list);
            }
        }

        //public static void Kimono(int count)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        _kimonoProxy.VoidWith3Params("hello", 20, _list);
        //    }
        //}

        public static void KimonoCore(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _kimonoProxyCore.VoidWith3Params("hello", 20, _list);
            }
        }

        void IInterceptorMethodCalls.Castle(int count) => Castle(count);

        void IInterceptorMethodCalls.Dotnet(int count) => Dotnet(count);

        //void IInterceptorMethodCalls.Kimono(int count) => Kimono(count);

        void IInterceptorMethodCalls.KimonoCore(int count) => KimonoCore(count);
    }
}
