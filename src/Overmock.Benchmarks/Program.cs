// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Overmock.Benchmarks;


BenchmarkRunner.Run<TypeInterceptorBenchmark>();
BenchmarkRunner.Run<NewProxyCreationBenchmark>();

//var benchmark = new TypeInterceptorBenchmark();
//benchmark.TypeInterceptor();
//benchmark.Decorator();
//benchmark.CastleProxy();

//benchmark.SimpleMockProxy();
//benchmark.MoqProxy();