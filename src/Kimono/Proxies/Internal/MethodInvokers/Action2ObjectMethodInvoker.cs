using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action2ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?>>
    {
        public Action2ObjectMethodInvoker(Func<Action<object?, object?>>  invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0]);

            return null;
        }
    }
}
