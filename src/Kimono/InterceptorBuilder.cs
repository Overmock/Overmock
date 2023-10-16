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
            return new InvocationChainHandler<T>(() => _invocationChain.GetEnumerator());
        }

        /// <inheritdoc/>
        public IDisposableInterceptor<T> Build<T>(T disposable) where T : class, IDisposable
        {
            return new DisposableInvocationChainHandler<T>(disposable, () => _invocationChain.GetEnumerator());
        }

        private sealed class InvocationChainHandler<T> : Interceptor<T> where T : class
        {
            private readonly Func<IEnumerator<InterceptorBuilderAction>> _chainProvider;

            public InvocationChainHandler(Func<IEnumerator<InterceptorBuilderAction>> handlersProvider)
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

        private sealed class DisposableInvocationChainHandler<T> : DisposableInterceptor<T> where T : class, IDisposable
        {
            private readonly Func<IEnumerator<InterceptorBuilderAction>> _chainProvider;

            public DisposableInvocationChainHandler(T disposable, Func<IEnumerator<InterceptorBuilderAction>> handlersProvider) : base(disposable)
            {
                _chainProvider = handlersProvider!;
            }

            protected override void Disposing(bool disposing)
            {
                if (disposing)
                {
                    Target?.Dispose();
                }
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
    }
}