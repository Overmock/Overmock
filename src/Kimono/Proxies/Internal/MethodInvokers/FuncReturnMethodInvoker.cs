using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class FuncReturnMethodInvoker : MethodDelegateInvoker<Func<object?>>
    {
        public FuncReturnMethodInvoker(Func<IInvocationContext, Func<object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod();
        }
    }
}
