using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action3ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?>>
    {
        public Action3ObjectMethodInvoker(Func<IInvocationContext, Action<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1]);

            return null;
        }
    }
}
