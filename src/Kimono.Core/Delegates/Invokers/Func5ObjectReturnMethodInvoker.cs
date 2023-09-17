using System;

namespace Kimono.Delegates.Invokers
{
    internal sealed class Func5ObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?, object?, object?, object?, object?>>
    {
        public Func5ObjectReturnMethodInvoker(Func<IInvocation, Func<object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3]);
        }
    }
}
