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

            _verifiables.Enqueue(result);

            return result;
        }

        public static void Verify()
        {
            while (_verifiables.TryDequeue(out var verifiable))
            {
                verifiable.Verify();
            }
        }

        internal static TMethod RegisterMethod<TMethod>(IOvermock overmock, TMethod property) where TMethod : IMethodCall
        {
            return overmock.AddMethod(property);
        }

        internal static TProperty RegisterProperty<TProperty>(IOvermock overmock, TProperty property) where TProperty : IPropertyCall
        {
            return overmock.AddProperty(property);
        }
    }

    internal class Overmocked<T> : Verifiable<T>, IOvermock<T>, IVerifiable<T> where T : class
    {
        private readonly List<IMethodCall> Methods = new List<IMethodCall>();
        private readonly List<IPropertyCall> Properties = new List<IPropertyCall>();

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

        public T Object => _lazyObject.Value;

        Type? IOvermock.GetCompiledType() => _compiledType;

        string IOvermock.TypeName => base.TypeName;

        protected override void Verify()
        {
            throw new VerifyException(this);
        }

        internal void SetCompiledType(Assembly assembly) => _compiledType = assembly.ExportedTypes.First(t => t.Name == TypeName);

        void IOvermock.SetCompiledType(Assembly assembly) => SetCompiledType(assembly);

        TMethod IOvermock.AddMethod<TMethod>(TMethod methodCall)
        {
            Methods.Add(methodCall);
            return methodCall;
        }

        TProperty IOvermock.AddProperty<TProperty>(TProperty methodCall)
        {
            Properties.Add(methodCall);
            return methodCall;
        }

        IEnumerable<IMethodCall> IOvermock.GetOvermockedMethods()
        {
            return Methods.AsReadOnly();
        }

        IEnumerable<IPropertyCall> IOvermock.GetOvermockedProperties()
        {
            return Properties.AsReadOnly();
        }
    }
}