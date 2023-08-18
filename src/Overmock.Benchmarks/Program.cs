using BenchmarkDotNet.Running;
using Overmock.Benchmarks;

BenchmarkRunner.Run<InterceptorMethodCallBenchmark>();
//BenchmarkRunner.Run<NewProxyCreationBenchmark>();

//new InterceptorMethodCallBenchmark().Kimono(100);

//var benchmark = new TypeInterceptorBenchmark();
//benchmark.TypeInterceptor();
//benchmark.Decorator();
//benchmark.CastleProxy();

//benchmark.SimpleMockProxy();
//benchmark.MoqProxy();