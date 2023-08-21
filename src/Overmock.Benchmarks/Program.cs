using BenchmarkDotNet.Running;
using Kimono;
using Overmock.Benchmarks;
using System.Diagnostics;

//BenchmarkRunner.Run<InterceptorMethodCallBenchmark>();
//BenchmarkRunner.Run<NewProxyCreationBenchmark>();

InterceptorMethodCalls.Kimono(1000); Console.ReadLine();



//benchmark.SimpleMockProxy();
//benchmark.MoqProxy();