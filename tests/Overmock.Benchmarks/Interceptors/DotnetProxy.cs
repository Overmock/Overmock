using Overmocked.Benchmarks.Models;
using System.Reflection;

namespace Overmocked.Benchmarks.Interceptors
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
