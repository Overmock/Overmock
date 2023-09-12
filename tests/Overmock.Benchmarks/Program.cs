using BenchmarkDotNet.Running;
using Overmocked.Benchmarks;

BenchmarkRunner.Run<InterceptorMethodCallBenchmark>();
BenchmarkRunner.Run<NewProxyCreationBenchmark>();

//new NewProxyCreationBenchmark().NewKimonoCore(10);
//InterceptorMethodCalls.KimonoCore(10); Console.ReadLine();