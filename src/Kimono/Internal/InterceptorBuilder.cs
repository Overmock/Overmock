using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Internal
{
    internal sealed class InterceptorBuilder : IInterceptorBuilder
    {
        private readonly List<InterceptorAction> _invocationChain = new List<InterceptorAction>();

        public IInterceptorBuilder Add(InterceptorAction action)
        {
            _invocationChain.Add(action);
            return this;
        }

        public IInterceptorBuilder Add(IInterceptorHandler handler)
        {
            return Add(handler.Handle);
        }

        public IInterceptor<T> Build<T>() where T : class
        {
            return new InvocationChainHandler<T>(() => _invocationChain.GetEnumerator());
        }

        private sealed class InvocationChainHandler<T> : Interceptor<T> where T : class
        {
            private readonly Func<IEnumerator<InterceptorAction>> _chainProvider;

            public InvocationChainHandler(Func<IEnumerator<InterceptorAction>> handlersProvider)
            {
                _chainProvider = handlersProvider!;
            }

            protected override void HandleInvocation(IInvocation invocation)
            {
                using var enumerator = _chainProvider();

                void Next(IInvocation invocation)
                {
                    if (enumerator.MoveNext())
                    {
                        enumerator.Current.Invoke(Next, invocation);
                    }
                }

                Next(invocation);
            }
        }

        private sealed class InvocationChainHandler2<T> : Interceptor<T> where T : class
        {
            private readonly InterceptorAction[] _chain;

            public InvocationChainHandler2(IEnumerable<InterceptorAction> handlers)
            {
                _chain = handlers.ToArray();
            }

            protected override void HandleInvocation(IInvocation invocation)
            {
                ref var current = ref MemoryMarshal.GetReference(_chain.AsSpan());

                if (current == null) { return; }

                Next(invocation, current, 0);
            }

            private void Next(IInvocation invocation, InterceptorAction firstHandler, int index)
            {
                if (index >= _chain.Length) { return; }

                ref var nextHandler = ref Unsafe.Add(ref firstHandler, ++index);

                if (nextHandler != null)
                {
                    nextHandler(c => Next(c, firstHandler, index), invocation);
                }
            }
        }
    }
}