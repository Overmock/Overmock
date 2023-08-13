using Kimono;
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
	private readonly CallbackInterceptor<T> _interceptor;
	private bool _overrideAll;

	/// <summary>
	/// Initializes a new instance of the <see cref="Overmock{T}" /> class.
	/// </summary>
	/// <exception cref="InvalidOperationException">Type '{Type.Name}' cannot be a sealed class or enum.</exception>
	public Overmock()
	{
		if (Type.IsSealed || Type.IsEnum)
		{
			throw new InvalidOperationException($"Type '{Type.Name}' cannot be a sealed class or enum.");
		}

		_interceptor = new CallbackInterceptor<T>(TargetMemberInvoked);

		Overmocked.Register(this);
	}

	/// <summary>
	/// Performs an implicit conversion from <see cref="Overmock{T}"/> to <typeparamref name="T"/>.
	/// </summary>
	/// <param name="overmock">The overmock.</param>
	/// <returns>The result of the conversion.</returns>
	public static implicit operator T(Overmock<T> overmock)
	{
		return overmock.Target;
	}

	/// <summary>
	/// Performs an implicit conversion from <typeparamref name="T"/> to <see cref="Overmock{T}"/>.
	/// </summary>
	/// <param name="target">The target.</param>
	/// <returns>The result of the conversion.</returns>
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

	/// <inheritdoc/>
	protected override void Verify()
	{
		throw new VerifyException(this);
	}

	/// <summary>
	/// Adds the method.
	/// </summary>
	/// <typeparam name="TMethod">The type of the t method.</typeparam>
	/// <param name="methodCall">The method call.</param>
	/// <returns>TMethod.</returns>
	TMethod IOvermock.AddMethod<TMethod>(TMethod methodCall)
	{
		_methods.Add(methodCall);
		return methodCall;
	}

	/// <inheritdoc />
	TProperty IOvermock.AddProperty<TProperty>(TProperty propertyCall)
	{
		_properties.Add(propertyCall);
		return propertyCall;
	}

	/// <inheritdoc />
	IEnumerable<IMethodCall> IOvermock.GetOvermockedMethods()
	{
		return _methods.AsReadOnly();
	}

	/// <inheritdoc />
	IEnumerable<IPropertyCall> IOvermock.GetOvermockedProperties()
	{
		return _properties.AsReadOnly();
	}

	/// <inheritdoc />
#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
	void IExpectAnyInvocation.ExpectAny(bool value = false)
#pragma warning restore CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
	{
		_overrideAll = value;
	}

	/// <summary>
	/// Targets the member invoked.
	/// </summary>
	/// <param name="context">The context.</param>
	/// <exception cref="Overmock.UnhandledMemberException"></exception>
	private void TargetMemberInvoked(IInvocationContext context)
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