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
        /// The target the built interceptor wraps, if one was supplied via
        /// <see cref="IInterceptorBuilder.Target{T}"/> or <see cref="IInterceptorBuilder.Dispose{T}"/>.
        /// Held as <see cref="object"/> so that the non-generic builder can carry it into
        /// <see cref="IInterceptorBuilder.Build{T}"/>.
        /// </summary>
        protected object? BuilderTarget { get; set; }

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
            // BuilderTarget is carried through so that a chain built as
            // .Target(t).AddCallback(...) does not lose its target: AddCallback returns the base
            // IInterceptorBuilder, so this overload - not ITargetedInterceptorBuilder.Build() - is
            // what the fluent chain resolves to.
            return new InvocationChainInterceptor<T>(BuilderTarget as T, InvocationChain);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected sealed class InvocationChainInterceptor<T> : Interceptor<T>, IDisposableInterceptor<T> where T : class
        {
            private readonly InterceptorBuilderAction[] _chain;
            private readonly T? _disposableTarget;

            /// <summary>
            ///
            /// </summary>
            /// <param name="handlers"></param>
            public InvocationChainInterceptor(IEnumerable<InterceptorBuilderAction> handlers) : this(null, handlers)
            {
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="disposable"></param>
            /// <param name="handlers"></param>
            public InvocationChainInterceptor(T? disposable, IEnumerable<InterceptorBuilderAction> handlers) : base(disposable)
            {
                if (handlers == null)
                {
                    throw new ArgumentNullException(nameof(handlers));
                }

                // Snapshot the chain. Holding an IEnumerator here instead meant the chain could only
                // ever be walked once for the lifetime of the interceptor: the second invocation
                // found the enumerator already exhausted and silently ran no handlers at all. An
                // array also makes the walk reentrant and safe to share across threads, which a
                // single shared enumerator was not.
                _chain = new List<InterceptorBuilderAction>(handlers).ToArray();
                _disposableTarget = disposable;
            }

            /// <summary>
            ///
            /// </summary>
            /// <param name="invocation"></param>
            protected override void HandleInvocation(IInvocation invocation)
            {
                // Position lives on the stack, so every invocation walks the chain from the start.
                Next(0, invocation);

                void Next(int index, IInvocation inv)
                {
                    if (index < _chain.Length)
                    {
                        _chain[index].Invoke(nextInvocation => Next(index + 1, nextInvocation), inv);
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
            BuilderTarget = target;
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
            return new InvocationChainInterceptor<T>(_target, InvocationChain);
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
            BuilderTarget = disposable;
            InvocationChain.AddRange(interceptorBuilder.GetInvocationChain());
        }

        /// <inheritdoc/>
        public IDisposableInterceptor<T> Build()
        {
            return new InvocationChainInterceptor<T>(_disposable, InvocationChain);
        }
    }
}
