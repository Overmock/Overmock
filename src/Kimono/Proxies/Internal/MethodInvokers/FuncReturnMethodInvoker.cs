using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class FuncReturnMethodInvoker : MethodDelegateInvoker<Func<object?>>
    {
        public FuncReturnMethodInvoker(Func<Func<object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return InvokeMethod();
        }
    }
}
