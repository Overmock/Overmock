using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action6ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?, object?, object?>>
    {
        public Action6ObjectMethodInvoker(Func<IInvocationContext, Action<object?, object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3], parameters[4]);

            return null;
        }
    }
}
