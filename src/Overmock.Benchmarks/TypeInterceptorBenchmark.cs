using BenchmarkDotNet.Attributes;
using Kimono;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Benchmarks
{
	public interface IBenchmarkTest
	{
		void VoidNoParams();
		bool BoolNoParams();
		object ObjectNoParams();
	}

	public class BenchmarkTest : IBenchmarkTest
	{
		public bool BoolNoParams()
		{
			return true;
		}

		public object ObjectNoParams()
		{
			return new object();
		}

		public void VoidNoParams()
		{
		}
	}

	public class BenchmarkDecorator : IBenchmarkTest
	{
		private readonly BenchmarkTest _benchmark;

		public BenchmarkDecorator(BenchmarkTest benchmark)
		{
			_benchmark = benchmark;
		}

		public bool BoolNoParams()
		{
			return _benchmark.BoolNoParams();
		}

		public object ObjectNoParams()
		{
			return _benchmark.ObjectNoParams();
		}

		public void VoidNoParams()
		{
			_benchmark.VoidNoParams();
		}
	}

	[MemoryDiagnoser]
	public class TypeInterceptorBenchmark
	{
		private static readonly BenchmarkTest _test = new BenchmarkTest();
		private static readonly IBenchmarkTest _benchmark = new BenchmarkDecorator(_test);
		private static readonly TypeInterceptor<IBenchmarkTest> _interceptor =
			Interceptor.Intercept<IBenchmarkTest, BenchmarkTest>(_test, c => c.InvokeTarget());
		private static readonly IBenchmarkTest _proxy = _interceptor.Proxy;

		[Benchmark]
		public void TypeInterceptor()
		{
			_proxy.VoidNoParams();
		}

		[Benchmark]
		public void Decorator()
		{
			_benchmark.VoidNoParams();
		}
	}
}
