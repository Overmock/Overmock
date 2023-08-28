using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Func6ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?, object?, object?, object?, object?>>
    {
        public Func6ObjectReturnMethodInvoker(Func<IInvocationContext, Func<object?, object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);
        }
    }
}
