using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Internal
{
    internal sealed class InvocationChainBuilder : IInvocationChainBuilder
    {
        private readonly List<InvocationChainAction> _invocationChain = new();

        public IInvocationChainBuilder Add(InvocationChainAction action)
        {
            _invocationChain.Add(action);
            return this;
        }

        public IInvocationChainBuilder Add(IInvocationChainHandler handler)
        {
            return Add(handler.Handle);
        }

        public IInvocationHandler Build()
        {
            return new InvocationChainHandler(() => _invocationChain.GetEnumerator());
            //return new InvocationChainHandler2(_invocationChain);
        }

        private sealed class InvocationChainHandler : IInvocationHandler
        {
            private readonly Func<IEnumerator<InvocationChainAction>> _chainProvider;

            public InvocationChainHandler(Func<IEnumerator<InvocationChainAction>> handlersProvider)
            {
                _chainProvider = handlersProvider;
            }

            public void Handle(IInvocationContext context)
            {
                using var enumerator = _chainProvider();

                void Next(IInvocationContext context)
                {
                    if (enumerator.MoveNext())
                    {
                        enumerator.Current.Invoke(Next, context);
                    }
                }

                Next(context);
            }
        }

        private sealed class InvocationChainHandler2 : IInvocationHandler
        {
            private readonly InvocationChainAction[] _chain;

            public InvocationChainHandler2(IEnumerable<InvocationChainAction> handlers)
            {
                _chain = handlers.ToArray();
            }

            public void Handle(IInvocationContext context)
            {
                ref var current = ref MemoryMarshal.GetReference(_chain.AsSpan());

                if (current == null) { return; }

                Next(context, current, 0);
            }

            private void Next(IInvocationContext context, InvocationChainAction firstHandler, int index)
            {
                if (index >= _chain.Length) { return; }

                ref var nextHandler = ref Unsafe.Add(ref firstHandler, ++index);

                if (nextHandler != null)
                {
                    nextHandler.Invoke(c => Next(c, firstHandler, index), context);
                }
            }
        }
    }
}