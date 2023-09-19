using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Func2ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?>>
    {
        public Func2ObjectReturnMethodInvoker(Func<IInvocationContext, Func<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0]);
        }
    }
}
