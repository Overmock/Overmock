using Overmock.Proxies;
using System.Reflection;

namespace Overmock;

/// <summary>
/// Allows for mocking classes and interfaces.
/// </summary>
/// <typeparam name="T">The type going to be mocked.</typeparam>
public class Overmock<T> : Verifiable<T>, IOvermock<T>, IExpectAnyInvocation where T : class
{
    private readonly List<IMethodCall> _methods = new List<IMethodCall>();
	private readonly List<IPropertyCall> _properties = new List<IPropertyCall>();
	private readonly TypeInterceptor<T> _interceptor;

	private Type? _compiledType;
	private bool _overrideAll;

	/// <summary>
	/// Initializes a new instance of the <see cref="Overmock{T}"/> class.
	/// </summary>
	/// <param name="factory"></param>
	/// <param name="argsProvider">The delegate used to get arguments to pass when constructing <typeparamref name="T" />.</param>
	public Overmock(Action<SetupArgs>? argsProvider = default)
	{
		if (Type.IsSealed || Type.IsEnum)
		{
			throw new InvalidOperationException($"Type '{Type.Name}' cannot be a sealed class or enum.");
		}

		_interceptor = new TypeInterceptor<T>(default, memberInvoked: TargetMemberInvoked);

		Overmocked.Register(this);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="overmock"></param>
	public static implicit operator T(Overmock<T> overmock)
	{
		return overmock.Target;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="target"></param>
	public static implicit operator Overmock<T>(T target)
	{
		if (target is Overmock<T> overmock)
		{
			return overmock;
		}

		return new Overmock<T>();
	}

	/// <summary>
	/// Gets the object.
	/// </summary>
	/// <value>The mocked object.</value>
	public T Target
    {
		get
		{
			return _interceptor.Proxy;
        }
    }

    Type? IOvermock.GetCompiledType() => _compiledType;

	string IOvermock.TypeName => _typeName;

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

	TProperty IOvermock.AddProperty<TProperty>(TProperty propertyCall)
	{
		_properties.Add(propertyCall);
		return propertyCall;
	}

	IEnumerable<IMethodCall> IOvermock.GetOvermockedMethods()
	{
		return _methods.AsReadOnly();
	}

	IEnumerable<IPropertyCall> IOvermock.GetOvermockedProperties()
	{
		return _properties.AsReadOnly();
	}

	void IExpectAnyInvocation.ExpectAny(bool value = false)
	{
		_overrideAll = value;
	}

	private void TargetMemberInvoked(InvocationContext context)
	{
		var methodCall = _methods.Find(m => m.BaseMethod == context.Method);

		if (methodCall != null)
		{
			var overmock = methodCall.GetOverrides().First();

			context.ReturnValue = overmock.Handle(new OvermockContext(context));
			
			return;
		}

		if (context.Member is PropertyInfo property)
		{
			var propertyCall = _properties.Find(p => p.PropertyInfo == property);

			if (propertyCall != null)
			{
				var overmock = propertyCall.GetOverrides().First();

				context.ReturnValue = overmock.Handle(new OvermockContext(context));
				
				return;
			}
		}

		if (!_overrideAll)
		{
			throw new UnhandledMemberException(context.MemberName);
		}
	}
}