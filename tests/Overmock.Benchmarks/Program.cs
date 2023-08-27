using BenchmarkDotNet.Running;
using Overmocked.Benchmarks;

BenchmarkRunner.Run<InterceptorMethodCallBenchmark>();
BenchmarkRunner.Run<NewProxyCreationBenchmark>();

//InterceptorMethodCalls.Kimono(1000); Console.ReadLine();