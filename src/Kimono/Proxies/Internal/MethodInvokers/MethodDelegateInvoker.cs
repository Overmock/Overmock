using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal abstract class MethodDelegateInvoker<TDelegate> : IMethodDelegateInvoker where TDelegate : Delegate
    {
        private TDelegate? _invokeMethod;
        private readonly Func<TDelegate> _delegeteGenerator;

        public MethodDelegateInvoker(Func<TDelegate> delegeteGenerator)
        {
            _delegeteGenerator = delegeteGenerator;
        }

        public TDelegate? InvokeMethod => _invokeMethod ??= _delegeteGenerator();

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
