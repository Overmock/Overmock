using BenchmarkDotNet.Attributes;
using Overmock.Benchmarks.Models;

namespace Overmock.Benchmarks
{
    [MemoryDiagnoser]
    public class InterceptorMethodCallBenchmark : IInterceptorMethodCalls
    {
        private readonly IInterceptorMethodCalls _methodCalls = new InterceptorMethodCalls();

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Castle(int count) => _methodCalls.Castle(count);

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Dotnet(int count) => _methodCalls.Dotnet(count);

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void Kimono(int count) => _methodCalls.Kimono(count);
    }
}
