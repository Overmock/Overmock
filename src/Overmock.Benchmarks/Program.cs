// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using Overmock.Benchmarks;


BenchmarkRunner.Run<TypeInterceptorBenchmark>();

//var benchmark = new TypeInterceptorBenchmark();
//benchmark.TypeInterceptor();