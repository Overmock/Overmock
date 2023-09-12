using System;

namespace Kimono.Core.Delegates.Invokers
{
    internal sealed class Action6ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?, object?, object?>>
    {
        public Action6ObjectMethodInvoker(Func<IInvocation, Action<object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);

            return null;
        }
    }
}
