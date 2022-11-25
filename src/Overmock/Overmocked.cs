using System.Collections.Concurrent;
using System.Reflection;

namespace Overmock
{
    public static class Overmocked
    {
        private static readonly ConcurrentQueue<IVerifiable> _verifiables = new ConcurrentQueue<IVerifiable>();

        static Overmocked()
        {
        }

        internal static IOvermockBuilder Builder
        {
            get { return OvermockBuilder.Instance; }
        }

        public static IOvermock<T> Setup<T>(Action<SetupArgs>? argsProvider = null) where T : class
        {
            var result = new Overmocked<T>(Builder.GetTypeBuilder(argsProvider));

            _verifiables.Append(result);

            return result;
        }

        internal static TMethod Register<TMethod>(IOvermock overmock, TMethod methodCall) where TMethod : IMethodCall
        {
            return overmock.AddMethod(methodCall);
        }
    }

    internal class Overmocked<T> : Verifiable<T>, IOvermock<T>, IVerifiable<T> where T : class
    {
        private readonly List<IMethodCall> Methods = new List<IMethodCall>();
        //private readonly List<IVerifiable<T>> Properties = new List<IVerifiable<T>>();

        private readonly Lazy<T> _lazyObject;
        private Type? _compiledType;

        internal Overmocked(ITypeBuilder builder)
        {
            var type = typeof(T);

            if (type.IsSealed || type.IsEnum)
            {
                throw new InvalidOperationException($"Type '{type.Name}' must be a non sealed/enum class.");
            }

            _lazyObject = new Lazy<T>(() => builder.BuildType(this));
        }

        internal Type? GetCompiledType() => _compiledType;

        Type? IOvermock<T>.GetCompiledType() => _compiledType;

        string IOvermock<T>.TypeName => base.TypeName;

        public T Object => _lazyObject.Value;

        internal void SetCompiledType(Assembly assembly) => _compiledType = assembly.ExportedTypes.First(t => t.Name == TypeName);

        void IOvermock<T>.SetCompiledType(Assembly assembly) => SetCompiledType(assembly);

        TMethod IOvermock.AddMethod<TMethod>(TMethod methodCall)
        {
            Methods.Add(methodCall);
            return methodCall;
        }
    }
}