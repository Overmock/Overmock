using Overmock.Benchmarks.Models;
using System.Reflection;

namespace Overmock.Benchmarks.Interceptors
{
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
