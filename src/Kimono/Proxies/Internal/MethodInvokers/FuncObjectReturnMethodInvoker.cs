using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class FuncObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?>>
    {
        public FuncObjectReturnMethodInvoker(Func<IInvocationContext, Func<object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target);
        }
    }
}
