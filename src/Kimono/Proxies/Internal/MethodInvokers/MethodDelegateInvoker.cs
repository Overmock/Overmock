using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IDelegateInvoker where TDelegate : Delegate
    {
        private TDelegate? _invokeMethod;
        private readonly Func<TDelegate> _delegateGenerator;

        public MethodDelegateInvoker(Func<TDelegate> delegateGenerator)
        {
            _delegateGenerator = delegateGenerator;
        }

        public TDelegate? InvokeMethod => _invokeMethod ??= _delegateGenerator();

        public abstract object? Invoke(object? target, params object?[] parameters);
    }

    internal sealed class ObjectArrayArgsDelegateInvoker : MethodDelegateInvoker<Func<object?, object?[], object?>>
    {
        public ObjectArrayArgsDelegateInvoker(Func<Func<object?, object?[], object?>> delegeteGenerator) : base(delegeteGenerator)
        {
        }

        public override object? Invoke(object? target, object?[] parameters)
        {
            return InvokeMethod(target, parameters);
        }
    }
}
