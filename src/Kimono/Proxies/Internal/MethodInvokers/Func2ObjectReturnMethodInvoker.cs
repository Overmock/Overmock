using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Func2ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?>>
    {
        public Func2ObjectReturnMethodInvoker(Func<Func<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0]);
        }
    }
}
