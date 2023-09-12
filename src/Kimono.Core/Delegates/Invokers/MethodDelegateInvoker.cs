using Kimono.Delegates;
using System;

namespace Kimono.Core.Delegates.Invokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IDelegateInvoker where TDelegate : Delegate
    {
        private TDelegate? _invokeMethod;
        private readonly Func<IInvocation, TDelegate> _delegateGenerator;

        public MethodDelegateInvoker(Func<IInvocation, TDelegate> delegateGenerator)
        {
            _delegateGenerator = delegateGenerator;
        }

        public TDelegate? InvokeMethod => _invokeMethod;

        public Func<IInvocation, TDelegate> DelegateGenerator => _delegateGenerator;

        public virtual object? Invoke(object? target, IInvocation context, params object?[] parameters)
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
