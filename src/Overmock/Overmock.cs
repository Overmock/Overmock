﻿using Kimono;
using Kimono.Interceptors;
using System.Reflection;

namespace Overmock;

/// <summary>
/// Allows for mocking classes and interfaces.
/// </summary>
/// <typeparam name="T">The type going to be mocked.</typeparam>
public class Overmock<T> : Verifiable<T>, IOvermock<T>, IExpectAnyInvocation, IEquatable<Overmock<T>> where T : class
{
	private readonly List<IMethodCall> _methods = new List<IMethodCall>();
	private readonly List<IPropertyCall> _properties = new List<IPropertyCall>();
	private readonly HandlerInterceptor<T> _interceptor;
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

		_interceptor = new HandlerInterceptor<T>(
            new OvermockInstanceInvocationHandler(() => _overrideAll, () => _methods, () => _properties)
        );

		Overmocked.Register(this);
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

    /// <summary>
    /// Implements the == operator.
    /// </summary>
    /// <param name="overmock">The overmock.</param>
    /// <param name="other">The other.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Overmock<T> overmock, object other)
    {
        return overmock.Equals(other);
    }

    /// <summary>
    /// Implements the != operator.
    /// </summary>
    /// <param name="overmock">The overmock.</param>
    /// <param name="other">The other.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Overmock<T> overmock, object other)
    {
        return !overmock.Equals(other);
    }

    /// <summary>
    /// Implements the == operator.
    /// </summary>
    /// <param name="overmock">The overmock.</param>
    /// <param name="other">The other.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(object other, Overmock<T> overmock)
    {
        return overmock.Equals(other);
    }

    /// <summary>
    /// Implements the != operator.
    /// </summary>
    /// <param name="overmock">The overmock.</param>
    /// <param name="other">The other.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(object other, Overmock<T> overmock)
    {
        return !overmock.Equals(other);
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

        if (Overmocked.IsRegistered(target))
        {
            return (Overmock<T>)Overmocked.GetRegistration(target)!;
        }

        return new Overmock<T>();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return Target == obj;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Target.GetHashCode();
    }

    /// <inheritdoc/>
    public bool Equals(Overmock<T>? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        if (ReferenceEquals(obj, null))
        {
            return false;
        }

        return Target == obj.Target;
    }

    /// <inheritdoc/>
    protected override void Verify()
	{
        foreach (var method in _methods)
        {
            method.Verify();
        }

        foreach (var property in _properties)
        {
            property.Verify();
        }
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

    /// <summary>
    /// Gets the target.
    /// </summary>
    /// <returns>System.Object.</returns>
    object IOvermock.GetTarget()
    {
        return Target;
    }

    /// <inheritdoc />
#pragma warning disable CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
    void IExpectAnyInvocation.ExpectAny(bool value = false)
#pragma warning restore CS1066 // The default value specified will have no effect because it applies to a member that is used in contexts that do not allow optional arguments
	{
		_overrideAll = value;
	}

    private sealed class OvermockInstanceInvocationHandler : IInvocationHandler
	{
		private readonly Func<bool> _expectAnyProvider;
		private readonly Func<List<IMethodCall>> _methodsProvider;
		private readonly Func<List<IPropertyCall>> _propertiesProvider;

		public OvermockInstanceInvocationHandler(Func<bool> expectAnyProvider, Func<List<IMethodCall>> methodsProvider, Func<List<IPropertyCall>> propertiesProvider)
		{
			_expectAnyProvider = expectAnyProvider;
			_methodsProvider = methodsProvider;
			_propertiesProvider = propertiesProvider;
		}

		public void Handle(IInvocationContext context)
		{
			var methods = _methodsProvider();

			var methodCall = methods.Find(m => m.BaseMethod == context.Method);

			if (methodCall != null)
			{
				var overmock = methodCall.GetOverrides().First();

				context.ReturnValue = overmock.Handle(new OvermockContext(context));

				return;
			}

			if (context.Member is PropertyInfo property)
			{
				var properties = _propertiesProvider();

				var propertyCall = properties.Find(p => p.PropertyInfo == property);

				if (propertyCall != null)
				{
					var overmock = propertyCall.GetOverrides().First();

					context.ReturnValue = overmock.Handle(new OvermockContext(context));

					return;
				}
			}

			if (!_expectAnyProvider())
			{
				throw new UnhandledMemberException(context.MemberName);
			}
		}
	}
}