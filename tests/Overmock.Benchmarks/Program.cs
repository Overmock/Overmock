﻿using BenchmarkDotNet.Running;
using Overmock.Benchmarks;

BenchmarkRunner.Run<InterceptorMethodCallBenchmark>();
//BenchmarkRunner.Run<NewProxyCreationBenchmark>();

//InterceptorMethodCalls.Kimono(1000); Console.ReadLine();



//benchmark.SimpleMockProxy();
//benchmark.MoqProxy();