using System;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class InterceptorBuilder : IInterceptorBuilder
    {
        private readonly List<InterceptorBuilderAction> _invocationChain = new List<InterceptorBuilderAction>();

        /// <inheritdoc/>
        public IInterceptorBuilder Add(InterceptorBuilderAction action)
        {
            _invocationChain.Add(action);
            return this;
        }

        /// <inheritdoc/>
        public IInterceptorBuilder Add(IInterceptorHandler handler)
        {
            return Add(handler.Handle);
        }

        /// <inheritdoc/>
        public IInterceptor<T> Build<T>() where T : class
        {
            return new InvocationChainHandler<T>(_invocationChain.GetEnumerator());
        }

        /// <inheritdoc/>
        public IDisposableInterceptor<T> Build<T>(T disposable) where T : class, IDisposable
        {
            return new InvocationChainHandler<T>(disposable, _invocationChain.GetEnumerator());
        }

        private sealed class InvocationChainHandler<T> : Interceptor<T>, IDisposableInterceptor<T> where T : class
        {
            private readonly IEnumerator<InterceptorBuilderAction> _chainProvider;
            private readonly T? _disposableTarget;

            public InvocationChainHandler(IEnumerator<InterceptorBuilderAction> handlersProvider) : this(null, handlersProvider)
            {
            }

            public InvocationChainHandler(T? disposable, IEnumerator<InterceptorBuilderAction> handlersProvider) : base(disposable)
            {
                _chainProvider = handlersProvider!;
                _disposableTarget = disposable;
            }

            protected override void HandleInvocation(IInvocation invocation)
            {
                var enumerator = _chainProvider;

                Next(invocation);

                return;

                void Next(IInvocation inv)
                {
                    if (enumerator.MoveNext())
                    {
                        enumerator.Current?.Invoke(Next, inv);
                    }
                }
            }

            public void Dispose()
            {
                (_disposableTarget as IDisposable)?.Dispose();
            }
        }
    }
}