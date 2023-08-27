﻿using Castle.DynamicProxy;
using Kimono;
using Overmock.Benchmarks.Interceptors;
using Overmock.Benchmarks.Models;
using System.Reflection;

namespace Overmock.Benchmarks
{
    public class NewProxyCreations : INewProxyCreations
    {
        private static readonly Benchmark _benchmarkClass = new Benchmark();

        public void NewKimono(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var kimonoProxy = Intercept.WithHandler<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());
            }
        }

        public void NewDotnet(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();
            }
        }

        public void NewCastle(int count)
        {
            var generator = new ProxyGenerator();

            for (int i = 0; i < count; i++)
            {
                var castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, new[] { new CastleInterceptor() });
            }
        }
    }
}