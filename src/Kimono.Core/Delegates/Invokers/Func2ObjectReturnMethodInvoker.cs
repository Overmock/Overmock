using System;

namespace Kimono.Core.Delegates.Invokers
{
    internal sealed class Func2ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?>>
    {
        public Func2ObjectReturnMethodInvoker(Func<IInvocation, Func<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0]);
        }
    }
}
