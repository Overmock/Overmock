using System;

namespace Kimono.Delegates.Invokers
{
    internal sealed class FuncReturnMethodInvoker : MethodDelegateInvoker<Func<object?>>
    {
        public FuncReturnMethodInvoker(Func<IInvocation, Func<object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod();
        }
    }
}
