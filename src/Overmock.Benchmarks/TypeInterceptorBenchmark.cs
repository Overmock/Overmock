using BenchmarkDotNet.Attributes;
using Castle.DynamicProxy;
using Kimono;
using System.Reflection;

namespace Overmock.Benchmarks
{
	public interface IBenchmark
	{
		void VoidNoParams();
		bool BoolNoParams();
		object ObjectNoParams();
        void VoidWith3Params(string name, int age, List<string> list);

    }

	public class Benchmark : IBenchmark
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

        public void VoidWith3Params(string name, int age, List<string> list)
        {
        }
	}

	[MemoryDiagnoser]
    [HardwareCounters]
	public class TypeInterceptorBenchmark
	{
		private static readonly Benchmark _benchmarkClass = new Benchmark();
		private static readonly IBenchmark _kimonoProxy;
		private static readonly IBenchmark _dispatchProxy;
		private static readonly IBenchmark _castleProxy;

		static TypeInterceptorBenchmark()
		{
			_kimonoProxy = Intercept.WithHandlers<IBenchmark, Benchmark>(_benchmarkClass, new KimonoInvocationHandler());

            _dispatchProxy = DispatchProxy.Create<IBenchmark, DotnetProxy>();

            var generator = new ProxyGenerator();
            var interceptors = new List<Castle.DynamicProxy.IInterceptor> { new CastleInterceptor() };
			_castleProxy = generator.CreateInterfaceProxyWithTarget<IBenchmark>(_benchmarkClass, interceptors.ToArray());
		}

		[Benchmark]
		public void Kimono()
		{
			_kimonoProxy.VoidNoParams();
		}

		[Benchmark]
		public void Dotnet()
		{
			_dispatchProxy.VoidNoParams();
		}

		[Benchmark]
		public void Castle()
		{
			_castleProxy.VoidNoParams();
		}
	}

	public class CastleInterceptor : Castle.DynamicProxy.IInterceptor
	{
		public void Intercept(IInvocation invocation)
		{
			invocation.Proceed();
		}
    }

    public class KimonoInvocationHandler : IInvocationHandler
    {
        public void Handle(IInvocationContext context)
        {
            context.Invoke();
        }
    }

    public class DotnetProxy : DispatchProxy
    {
        public IBenchmark? Benchmark { get; set; }

        protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
        {
            Benchmark?.VoidNoParams();

            return null;
        }
    }
}
