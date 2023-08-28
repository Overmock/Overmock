using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IDelegateInvoker where TDelegate : Delegate
    {
        private TDelegate? _invokeMethod;
        private readonly Func<IInvocationContext, TDelegate> _delegateGenerator;

        public MethodDelegateInvoker(Func<IInvocationContext, TDelegate> delegateGenerator)
        {
            _delegateGenerator = delegateGenerator;
        }

        public TDelegate? InvokeMethod => _invokeMethod;

        public Func<IInvocationContext, TDelegate> DelegateGenerator => _delegateGenerator;

        public virtual object? Invoke(object? target, IInvocationContext context, params object?[] parameters)
        {
            var generator = DelegateGenerator;

            if (context.Method.IsGenericMethod)
            {
                _invokeMethod = generator(context);

                return InvokeCore(target, parameters);
            }

            if (_invokeMethod is null)
            {
                _invokeMethod = generator(context);
            }

            return InvokeCore(target, parameters);
        }

        protected abstract object? InvokeCore(object? target, params object?[] parameters);
    }
}
