using BenchmarkDotNet.Attributes;
using Castle.DynamicProxy;
using Kimono;
using Overmock.Benchmarks.Interceptors;
using Overmock.Benchmarks.Models;
using System.Reflection;

namespace Overmock.Benchmarks
{

    [MemoryDiagnoser]
    [HardwareCounters]
	public class TypeInterceptorBenchmark
	{
		private static readonly Benchmark _benchmarkClass = new Benchmark();
		private static readonly IBenchmark _kimonoProxy;
		private static readonly IBenchmark _dispatchProxy;
		private static readonly IBenchmark _castleProxy;

		static TypeInterceptorBenchmark()
		{
			_kimonoProxy = Intercept.WithHandlers<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());

            _dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();

            var generator = new ProxyGenerator();
            var interceptors = new List<Castle.DynamicProxy.IInterceptor> { new CastleInterceptor() };
			_castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, interceptors.ToArray());
		}

		[Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Kimono(int count)
		{
            for (int i = 0; i < count; i++)
            {
                _kimonoProxy.VoidWith3Params("hello", 20, new List<string> { "world" });
            }
		}

		[Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Dotnet(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _dispatchProxy.VoidWith3Params("hello", 20, new List<string> { "world" });
            }
        }

		[Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Castle(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _castleProxy.VoidWith3Params("hello", 20, new List<string> { "world" });
            }
		}
	}
}
