using System;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public class InterceptorBuilder : IInterceptorBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly List<InterceptorBuilderAction> InvocationChain = new List<InterceptorBuilderAction>();

        /// <summary>
        /// 
        /// </summary>
        internal List<InterceptorBuilderAction> GetInvocationChain() => InvocationChain;

        /// <inheritdoc/>
        public IInterceptorBuilder AddCallback(InterceptorBuilderAction action)
        {
            GetInvocationChain().Add(action);
            return this;
        }

        /// <inheritdoc/>
        public IInterceptorBuilder AddHandler(IInvocationHandler handler)
        {
            return AddCallback(handler.Handle);
        }

        /// <inheritdoc />
        IDisposableInterceptorBuilder<T> IInterceptorBuilder.Dispose<T>(T disposable)
        {
            return new DisposableInterceptorBuilder<T>(disposable, this);
        }

        /// <inheritdoc />
        ITargetedInterceptorBuilder<T> IInterceptorBuilder.Target<T>(T target)
        {
            return new TargetedInterceptorBuilder<T>(target, this);
        }

        /// <inheritdoc/>
        IInterceptor<T> IInterceptorBuilder.Build<T>()
        {
            return new InvocationChainInterceptor<T>(InvocationChain.GetEnumerator());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected sealed class InvocationChainInterceptor<T> : Interceptor<T>, IDisposableInterceptor<T> where T : class
        {
            private readonly IEnumerator<InterceptorBuilderAction> _chainEnumerator;
            private readonly T? _disposableTarget;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="handlersProvider"></param>
            public InvocationChainInterceptor(IEnumerator<InterceptorBuilderAction> handlersProvider) : this(null, handlersProvider)
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="disposable"></param>
            /// <param name="handlersProvider"></param>
            public InvocationChainInterceptor(T? disposable, IEnumerator<InterceptorBuilderAction> handlersProvider) : base(disposable)
            {
                _chainEnumerator = handlersProvider;
                _disposableTarget = disposable;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="invocation"></param>
            protected override void HandleInvocation(IInvocation invocation)
            {
                Next(invocation);

                return;

                void Next(IInvocation inv)
                {
                    if (_chainEnumerator.MoveNext())
                    {
                        _chainEnumerator.Current.Invoke(Next, inv);
                    }
                }
            }

            /// <summary>
            /// 
            /// </summary>
            public void Dispose()
            {
                (_disposableTarget as IDisposable)?.Dispose();
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class TargetedInterceptorBuilder<T> : InterceptorBuilder, ITargetedInterceptorBuilder<T> where T : class
    {
        private readonly T _target;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="interceptorBuilder"></param>
        public TargetedInterceptorBuilder(T target, InterceptorBuilder interceptorBuilder)
        {
            _target = target;
            InvocationChain.AddRange(interceptorBuilder.GetInvocationChain());
        }

        /// <inheritdoc/>
        public IDisposableInterceptorBuilder<T> DisposeResources()
        {
            return new DisposableInterceptorBuilder<T>(_target, this);
        }

        /// <inheritdoc/>
        public IInterceptor<T> Build()
        {
            return new InvocationChainInterceptor<T>(_target, InvocationChain.GetEnumerator());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class DisposableInterceptorBuilder<T> : InterceptorBuilder, IDisposableInterceptorBuilder<T> where T : class//, IDisposable
    {
        private readonly T _disposable;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposable"></param>
        /// <param name="interceptorBuilder"></param>
        public DisposableInterceptorBuilder(T disposable, InterceptorBuilder interceptorBuilder)
        {
            _disposable = disposable;
            InvocationChain.AddRange(interceptorBuilder.GetInvocationChain());
        }

        /// <inheritdoc/>
        public IDisposableInterceptor<T> Build()
        {
            return new InvocationChainInterceptor<T>(_disposable, InvocationChain.GetEnumerator());
        }
    }
}