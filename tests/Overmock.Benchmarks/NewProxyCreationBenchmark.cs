using BenchmarkDotNet.Attributes;
using Overmocked.Benchmarks.Models;

namespace Overmocked.Benchmarks
{
    [MemoryDiagnoser]
    public class NewProxyCreationBenchmark : INewProxyCreations
    {
        private static readonly INewProxyCreations _creations = new NewProxyCreations();

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewKimono(int count) => _creations.NewKimono(count);

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewDotnet(int count) => _creations.NewDotnet(count);

        [Benchmark]
        [Arguments(1_000)]
        [Arguments(1_000_000)]
        //[Arguments(100_000_000)]
        public void NewCastle(int count) => _creations.NewCastle(count);
    }
}
