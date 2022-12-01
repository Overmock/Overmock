using System.ComponentModel;
using System.Reflection;

namespace Overmock
{
    public interface IOvermock : IVerifiable
    {
        TMethod AddMethod<TMethod>(TMethod method) where TMethod : IMethodCall;

        TProperty AddProperty<TProperty>(TProperty property) where TProperty : IPropertyCall;

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

    /// <summary>
    /// Represents a mocked type who's members can be overridden.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Overmock.Mocking.IVerifiable" />
    public interface IOvermock<T> : IVerifiable<T>, IOvermock where T : class
    {
        /// <summary>
        /// Gets the mocked object.
        /// </summary>
        /// <value>The mocked object.</value>
        T? Object { get; }
    }
}