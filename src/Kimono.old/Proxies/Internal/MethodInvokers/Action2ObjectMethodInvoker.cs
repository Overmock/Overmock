using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action2ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?>>
    {
        public Action2ObjectMethodInvoker(Func<IInvocationContext, Action<object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0]);

            return null;
        }
    }
}
