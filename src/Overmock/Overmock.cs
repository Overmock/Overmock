using System.Reflection;
using Overmock.Runtime.Marshalling;

namespace Overmock;

/// <summary>
/// Allows for mocking classes and interfaces.
/// </summary>
/// <typeparam name="T">The type going to be mocked.</typeparam>
public class Overmock<T> : Verifiable<T>, IOvermock<T> where T : class
{
    private readonly List<IMethodCall> _methods = new List<IMethodCall>();
	private readonly List<IPropertyCall> _properties = new List<IPropertyCall>();
	private readonly Lazy<T> _lazyObject;

	private Type? _compiledType;

    /// <summary>
    /// Initializes a new instance of the <see cref="Overmock{T}"/> class.
    /// </summary>
    /// <param name="factory"></param>
    /// <param name="argsProvider">The delegate used to get arguments to pass when constructing <typeparamref name="T" />.</param>
    public Overmock(IMarshallerFactory? factory = null, Action<SetupArgs>? argsProvider = default)
    {
        Overmocked.Register(this);
		
        if (Type.IsSealed || Type.IsEnum)
        {
            throw new InvalidOperationException($"Type '{Type.Name}' must be a non sealed/enum class.");
        }

        _lazyObject = new Lazy<T>(() =>
        {
            var marshaller = (factory ?? Overmocked.GetMarshallerFactory()).Create(this);
            return marshaller.Marshal<T>() ?? throw new OvermockException("Can't believe this happened right now.");
        });
    }

	/// <summary>
	/// Gets the object.
	/// </summary>
	/// <value>The mocked object.</value>
	public T Target => _lazyObject.Value;

	Type? IOvermock.GetCompiledType() => _compiledType;

	string IOvermock.TypeName => base._typeName;

	/// <inheritdoc/>
	protected override void Verify()
	{
		throw new VerifyException(this);
	}

	internal void SetCompiledType(Type? compiledType) => _compiledType = compiledType;

	internal void SetCompiledType(Assembly assembly) => SetCompiledType(assembly.ExportedTypes.First(t => t.Name == _typeName));

	void IOvermock.SetCompiledType(Assembly assembly) => SetCompiledType(assembly);

	void IOvermock.SetCompiledType(Type compiledType) => SetCompiledType(compiledType);

	TMethod IOvermock.AddMethod<TMethod>(TMethod methodCall)
	{
		_methods.Add(methodCall);
		return methodCall;
	}

	TProperty IOvermock.AddProperty<TProperty>(TProperty methodCall)
	{
		_properties.Add(methodCall);
		return methodCall;
	}

	IEnumerable<IMethodCall> IOvermock.GetOvermockedMethods()
	{
		return _methods.AsReadOnly();
	}

	IEnumerable<IPropertyCall> IOvermock.GetOvermockedProperties()
	{
		return _properties.AsReadOnly();
	}
}