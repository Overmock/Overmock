﻿using Kimono;
using Overmocked.Mocking;
using Overmocked.Mocking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Overmocked
{
    /// <summary>
    /// Allows for mocking classes and interfaces.
    /// </summary>
    /// <typeparam name="T">The type going to be mocked.</typeparam>
    public class Overmock<T> : Verifiable<T>, IOvermock<T>, IOvermockable, IOvermocked, IExpectAnyInvocation, IEquatable<Overmock<T>> where T : class
    {
        private static readonly IProxyFactory _proxyFactory = ProxyFactory.Create();

        private readonly List<IMethodCall> _methods = new List<IMethodCall>();
        private readonly List<IPropertyCall> _properties = new List<IPropertyCall>();

        private readonly Interceptor<T> _interceptor;
        private readonly T _target;
        private bool _overrideAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="Overmock{T}" /> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Type '{Type.Name}' cannot be a sealed class or enum.</exception>
        public Overmock(IInvocationHandler? handler = null)
        {
            //_invocationHandler = new OvermockInstanceInvocationHandler(() => _overrideAll, _methods.ToArray, _properties.ToArray);
            _interceptor = new OvermockInterceptor<T>(() => _overrideAll, _methods.ToArray, _properties.ToArray, handler);
            _target = _proxyFactory.CreateInterfaceProxy(_interceptor);

            if (Type.IsSealed || Type.IsEnum)
            {
                throw new InvalidOperationException($"Type '{Type.Name}' cannot be a sealed class or enum.");
            }

            Overmock.Register(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Overmock{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <exception cref="System.InvalidOperationException">Type '{Type.Name}' cannot be a sealed class or enum.</exception>
        internal Overmock(T target)
        {
            if (Type.IsSealed || Type.IsEnum)
            {
                throw new InvalidOperationException($"Type '{Type.Name}' cannot be a sealed class or enum.");
            }

            if (target is IProxy proxy)
            {
                var overmock = Overmock.GetRegistration(proxy)!;
                _methods = (List<IMethodCall>)((IOvermocked)overmock).GetMethods();
                _properties = (List<IPropertyCall>)((IOvermocked)overmock).GetProperties();
            }

            _interceptor = new OvermockInterceptor<T>(() => _overrideAll, _methods.ToArray, _properties.ToArray, null);
            _target = _proxyFactory.CreateInterfaceProxy(_interceptor);

            Overmock.Register(this);
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
        /// Gets the object.
        /// </summary>
        /// <value>The mocked object.</value>
        public T Target
        {
            get
            {
                return _target;
            }
        }

        ///// <summary>
        ///// Performs an implicit conversion from <see cref="Overmock{T}"/> to <typeparamref name="T"/>.
        ///// </summary>
        ///// <param name="overmock">The overmock.</param>
        ///// <returns>The result of the conversion.</returns>
        //public static implicit operator Overmock<T>(T overmock)
        //{
        //    return overmock.Target;
        //}

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj is null)
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

            if (obj is null)
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
        TMethod IOvermockable.AddMethod<TMethod>(TMethod methodCall)
        {
            _methods.Add(methodCall);
            return methodCall;
        }

        /// <inheritdoc />
        TProperty IOvermockable.AddProperty<TProperty>(TProperty propertyCall)
        {
            _properties.Add(propertyCall);
            return propertyCall;
        }

        /// <inheritdoc />
        IEnumerable<IMethodCall> IOvermocked.GetMethods()
        {
            return _methods;
        }

        /// <inheritdoc />
        IEnumerable<IPropertyCall> IOvermocked.GetProperties()
        {
            return _properties;
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
        void IExpectAnyInvocation.ExpectAny()
        {
            _overrideAll = true;
        }
    }
}