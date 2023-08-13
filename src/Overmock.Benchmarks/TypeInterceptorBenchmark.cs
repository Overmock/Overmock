using BenchmarkDotNet.Attributes;
using Castle.DynamicProxy;
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
		private static readonly BenchmarkTest _benchmarkClass = new BenchmarkTest();

		private static readonly IBenchmarkTest _kimonoProxy;

		private static readonly IBenchmarkTest _benchmarkDecerator;

		//private static readonly Moq.Mock<IBenchmarkTest> _moq;
		//private static readonly IBenchmarkTest _moqProxy;

		//private static readonly IBenchmarkTest _simplemock;

		private static readonly IBenchmarkTest _castleProxy;

		static TypeInterceptorBenchmark()
		{
			_kimonoProxy = Interceptor.TargetedWithCallback<IBenchmarkTest, BenchmarkTest>(_benchmarkClass, c =>
			{
				c.InvokeTarget();
			});

			_benchmarkDecerator = new BenchmarkDecorator(_benchmarkClass);

			var interceptors = new List<Castle.DynamicProxy.IInterceptor>();
			var voidMethodInterceptor = new MethodInterceptor();
			interceptors.Add(voidMethodInterceptor);

			var generator = new Castle.DynamicProxy.ProxyGenerator();
			_castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmarkTest>(_benchmarkClass, interceptors.ToArray());

			//_moq = new Moq.Mock<IBenchmarkTest>();
			//_moqProxy = _moq.Object;

			//_simplemock = Simple.Mocking.Mock.Interface<IBenchmarkTest>();
			//Simple.Mocking.Expect.MethodCall(() => _simplemock.VoidNoParams()).Executes(() =>
			//{
			//	_benchmarkClass.VoidNoParams();
			//});

			//_moq.Setup(m => m.VoidNoParams()).Callback(new Moq.InvocationAction(i =>
			//{
			//	_benchmarkClass.VoidNoParams();
			//}));

			//_moqProxy = _moq.Object;
		}

		[Benchmark]
		public void TypeInterceptor()
		{
			_kimonoProxy.VoidNoParams();
		}

		[Benchmark]
		public void Decorator()
		{
			_benchmarkClass.VoidNoParams();
		}

		[Benchmark]
		public void CastleProxy()
		{
			_castleProxy.VoidNoParams();
		}

		//[Benchmark]
		//public void SimpleMockProxy()
		//{
		//	_simplemock.VoidNoParams();
		////}

		//[Benchmark]
		//public void MoqProxy()
		//{
		//	_moqProxy.VoidNoParams();
		//}
	}

	public class MethodInterceptor : Castle.DynamicProxy.IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();
		}
	}
}
