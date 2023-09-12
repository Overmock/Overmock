using Kimono.Core;
using Overmocked.Mocking;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Overmocked
{
    /// <summary>
    /// Contains methods used for configuring an overmock.
    /// </summary>
    public static partial class Overmock
    {
        private static readonly ConcurrentQueue<IOvermock> _overmocks = new ConcurrentQueue<IOvermock>();

        private static IInterceptor? _invocationHandler;

        /// <summary>
        /// Initializes static members of the <see cref="global::Overmocked.Overmock"/> class.
        /// </summary>
        static Overmock()
        {
        }

        ///// <summary>
        ///// Uses the specified handler.
        ///// </summary>
        ///// <param name="handler">The handler.</param>
        //public static void Use(IInvocationHandler handler)
        //{
        //    Interlocked.Exchange(ref _invocationHandler, handler);
        //}

        /// <summary>
        /// Verifies the mocks setup behave as expected.
        /// </summary>
        public static void Verify()
        {
            while (_overmocks.TryDequeue(out var overmock))
            {
                overmock.Verify();
            }
        }

        internal static Overmock<T> GetOvermock<T>([NotNull] T target) where T : class
        {
            Overmock<T>? overmock = null;

            if (target is Overmock<T> mock)
            {
                return mock;
            }

            if (IsRegistered(target))
            {
                overmock = (Overmock<T>)GetRegistration(target)!;
            }

            if (typeof(T).Implements<IProxy>())
            {
                throw new KimonoException($"{typeof(T)} must be one of ({nameof(IOvermock<T>.Target)}, {nameof(IOvermock<T>)})");
            }

            if (overmock is null)
            {
                throw new KimonoException($"{typeof(T)} cannot be used as an overmock.");
            }

            return overmock;
        }

        internal static void Register<T>(IOvermock<T> overmock) where T : class
        {
            _overmocks.Enqueue(overmock);
        }

        internal static bool IsRegistered<T>(T target) where T : class
        {
            return _overmocks.Any(o => o.GetTarget() == target);
        }

        internal static IOvermock? GetRegistration<T>(T target) where T : class
        {
            return _overmocks.FirstOrDefault(o => o.GetTarget() == target);
        }

        internal static IOvermock? GetRegistration<T>(IOvermock<T> target) where T : class
        {
            return _overmocks.FirstOrDefault(o => o == target);
        }

        internal static IMethodCall<T> RegisterMethod<T>(IOvermockable overmock, IMethodCall<T> method) where T : class
        {
            return overmock.AddMethod(method);
        }

        internal static IMethodCall<T, TReturn> RegisterMethod<T, TReturn>(IOvermockable overmock, IMethodCall<T, TReturn> method) where T : class
        {
            return overmock.AddMethod(method);
        }

        internal static IPropertyCall<T, TProperty> RegisterProperty<T, TProperty>(IOvermockable overmock, IPropertyCall<T, TProperty> property) where T : class
        {
            return overmock.AddProperty(property);
        }
    }
}