using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono.Interceptors
{
	/// <summary>
	/// Class HandlersInterceptor.
	/// Implements the <see cref="Interceptor{T}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Interceptor{T}" />
	public class HandlersInterceptor<T> : Interceptor<T> where T : class
    {
        private readonly Lazy<IInvocationHandler[]> _interceptorsLazy;

        /// <summary>
        /// Initializes a new instance of the <see cref="HandlersInterceptor{T}"/> class.
        /// </summary>
        /// <param name="handlers">The handlers.</param>
        public HandlersInterceptor(IEnumerable<IInvocationHandler> handlers) : base()
        {
            IInvocationHandler[]? invocationHandlers = null;
            // wait to enumerate the collection until we need too.
            _interceptorsLazy = new Lazy<IInvocationHandler[]>(() => invocationHandlers ??= handlers.ToArray());
        }

        /// <inheritdoc/>
        protected override void MemberInvoked(IInvocationContext context)
        {
            Span<IInvocationHandler> handlers = _interceptorsLazy.Value;

            void InvokeHandlers(ref IInvocationHandler firstHandler, Span<IInvocationHandler> handlers)
            {
                if (firstHandler == null) { return; }

                for (int i = 0; i < handlers.Length; i++)
                {
                    ref var handler = ref Unsafe.Add(ref firstHandler, i);

                    handler.Handle(context);
                }
            }

            InvokeHandlers(ref MemoryMarshal.GetReference(handlers), handlers);
        }
    }
}
