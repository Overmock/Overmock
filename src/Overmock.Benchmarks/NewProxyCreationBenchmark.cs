using BenchmarkDotNet.Attributes;
using Castle.DynamicProxy;
using Kimono;
using Overmock.Benchmarks.Interceptors;
using Overmock.Benchmarks.Models;
using System.Reflection;

namespace Overmock.Benchmarks
{
    [MemoryDiagnoser]
	public class NewProxyCreationBenchmark
    {
        private static readonly Benchmark _benchmarkClass = new Benchmark();
		[Benchmark]
        [Arguments(1_000)]
        //[Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewKimono(int count)
		{
            for (int i = 0; i < count; i++)
            {
                var kimonoProxy = Intercept.WithHandler<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());
            }
		}

		[Benchmark]
        [Arguments(1_000)]
        //[Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewDotnet(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();
            }
        }

		[Benchmark]
        [Arguments(1_000)]
        //[Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewCastle(int count)
        {
            for (int i = 0; i < count; i++)
            {

                var generator = new ProxyGenerator();
                var interceptors = new List<Castle.DynamicProxy.IInterceptor> { new CastleInterceptor() };
                var castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, interceptors.ToArray());
            }
		}
	}
}
