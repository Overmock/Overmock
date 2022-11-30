using System.ComponentModel;
using System.Reflection;

namespace Overmock
{
    public interface IOvermock : IVerifiable
    {
        TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;

        TProperty AddProperty<TProperty>(TProperty property) where TProperty : IPropertyCall;
    }

    public interface IOvermock<T> : IVerifiable<T>, IOvermock where T : class
    {
        T Object { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        string TypeName { get; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        Type? GetCompiledType();

        [EditorBrowsable(EditorBrowsableState.Never)]
        void SetCompiledType(Assembly assembly);

        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IMethodCall> GetOvermockedMethods();

        [EditorBrowsable(EditorBrowsableState.Never)]
        IEnumerable<IPropertyCall> GetOvermockedProperties();
    }
}