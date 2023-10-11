using Kimono;
using Overmocked.Mocking;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Overmocked
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class OvermockInterceptor<T> : Interceptor<T> where T : class
    {
        private readonly Func<bool> _expectAnyProvider;
        private readonly Func<IMethodCall[]> _methodsProvider;
        private readonly Func<IPropertyCall[]> _propertiesProvider;
        private readonly IInvocationHandler? _handler;

        internal OvermockInterceptor(Func<bool> expectAnyProvider, Func<IMethodCall[]> methodsProvider, Func<IPropertyCall[]> propertiesProvider, IInvocationHandler? handler = null)
        {
            _expectAnyProvider = expectAnyProvider;
            _methodsProvider = methodsProvider;
            _propertiesProvider = propertiesProvider;
            _handler = handler;
        }

        internal static bool HandleMembers<TInfo, TCall>(IInvocation context, TInfo info, Span<TCall> overridables, Func<TInfo, TCall, bool> predicate, IInvocationHandler? handler = null) where TCall : IOverridable
        {
            ref var reference = ref MemoryMarshal.GetReference(overridables);

            if (reference == null) { return false; }

            for (int i = 0; i < overridables.Length; i++)
            {
                ref var call = ref Unsafe.Add(ref reference, i);

                if (!predicate(info, call)) { continue; }

                var overmock = call.GetOverrides().First();
                var overmockContext = new OvermockContext(context);

                if (call is IMethodCall method)
                {
                    HandleParameterMatching(method.Matches, context.Parameters);
                }

                handler?.Handle(overmockContext);
                context.ReturnValue = overmock.Handle(overmockContext);
                return true;
            }

            return false;
        }

        private static void HandleParameterMatching(IMatch[] matches, object?[]? parameters)
        {
            if (matches.Length > 0 && parameters is null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (matches.Length != parameters.Length)
            {
                throw new InvalidOperationException(
                    $"Parameters matching collection sizes do not match. Parameters.Length: {parameters.Length}, Matches.Length: {matches.Length}"
                );
            }

            for (int i = 0; i < matches.Length; i++)
            {
                if (!matches[i].Matches(parameters[i]!))
                {
                    throw new OvermockException($"Parameter '{parameters[i]}' does not match expected value.");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        /// <exception cref="UnhandledMemberException"></exception>
        protected override void HandleInvocation(IInvocation invocation)
        {
            if (HandleMethods(invocation, _methodsProvider())) { return; }

            var handled = false;

            if (invocation.IsProperty)
            {
                handled = HandleProperties(invocation, _propertiesProvider());
            }

            if (!handled && !_expectAnyProvider())
            {
                throw new UnhandledMemberException(invocation.Method.Name);
            }
        }

        private static bool HandleMethods(IInvocation context, Span<IMethodCall> methods, IInvocationHandler? handler = null)
        {
            return HandleMembers(context, context.Method, methods, (info, call) => info == call.BaseMethod, handler);
        }

        private static bool HandleProperties(IInvocation context, Span<IPropertyCall> properties, IInvocationHandler? handler = null)
        {
            return HandleMembers(context, context.Method, properties, (info, call) => {
                var property = call.PropertyInfo;
                return info == property.GetGetMethod()
                    || info == property.GetSetMethod();
            }, handler);
        }
    }
}