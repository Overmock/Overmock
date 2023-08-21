using Kimono;
using Kimono.Interceptors;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Overmock
{
    /// <summary>
    /// Allows for mocking classes and interfaces.
    /// </summary>
    /// <typeparam name="T">The type going to be mocked.</typeparam>
    public class Overmock<T> : Verifiable<T>, IOvermock<T>, IExpectAnyInvocation, IEquatable<Overmock<T>> where T : class
    {
        private readonly List<IMethodCall> _methods = new List<IMethodCall>();
        private readonly List<IPropertyCall> _properties = new List<IPropertyCall>();

        private readonly IInvocationHandler _invocationHandler;
        private readonly Interceptor<T> _interceptor;
        private bool _overrideAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="Overmock{T}" /> class.
        /// </summary>
        /// <exception cref="InvalidOperationException">Type '{Type.Name}' cannot be a sealed class or enum.</exception>
        public Overmock(IInvocationHandler? handler = null)
        {
            _invocationHandler = new OvermockInstanceInvocationHandler(() => _overrideAll, _methods.ToArray, _properties.ToArray);

            if (Type.IsSealed || Type.IsEnum)
            {
                throw new InvalidOperationException($"Type '{Type.Name}' cannot be a sealed class or enum.");
            }

            _interceptor = handler == null
                ? new HandlerInterceptor<T>(_invocationHandler)
                : new HandlersInterceptor<T>(new[] { _invocationHandler, handler });

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
        /// Performs an implicit conversion from <see cref="Overmock{T}"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="overmock">The overmock.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator T(Overmock<T> overmock)
        {
            return overmock.Target;
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
            private readonly Func<IMethodCall[]> _methodsProvider;
            private readonly Func<IPropertyCall[]> _propertiesProvider;

            internal OvermockInstanceInvocationHandler(Func<bool> expectAnyProvider, Func<IMethodCall[]> methodsProvider, Func<IPropertyCall[]> propertiesProvider)
            {
                _expectAnyProvider = expectAnyProvider;
                _methodsProvider = methodsProvider;
                _propertiesProvider = propertiesProvider;
            }
            
            internal static bool HandleMembers<TInfo, TCall>(IInvocationContext context, TInfo info, Span<TCall> overridables, Func<TInfo, TCall, bool> predicate) where TCall : IOverridable
            {
                ref var reference = ref MemoryMarshal.GetReference(overridables);

                if (reference == null) { return false; }

                for (int i = 0; i < overridables.Length; i++)
                {
                    ref var call = ref Unsafe.Add(ref reference, i);

                    if (!predicate(info, call)) { continue; }

                    var overmock = call.GetOverrides().First();
                    context.ReturnValue = overmock.Handle(new OvermockContext(context));
                    return true;
                }

                return false;
            }

            /// <exclude />
            public void Handle(IInvocationContext context)
            {
                if (HandleMethods(context, _methodsProvider())) { return; }

                var handled = false;

                if (context.Member is PropertyInfo property)
                {
                    handled = HandleProperties(context, property, _propertiesProvider());
                }

                if (!handled && !_expectAnyProvider())
                {
                    throw new UnhandledMemberException(context.MemberName);
                }
            }

            private static bool HandleMethods(IInvocationContext context, Span<IMethodCall> methods)
            {
                return HandleMembers(context, context.Method, methods, (info, call) => info == call.BaseMethod);
            }

            private static bool HandleProperties(IInvocationContext context, PropertyInfo propertyInfo, Span<IPropertyCall> properties)
            {
                return HandleMembers(context, propertyInfo, properties, (info, call) => info == call.PropertyInfo);
            }
        }
    }
}