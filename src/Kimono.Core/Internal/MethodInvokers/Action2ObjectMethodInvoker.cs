using System;

namespace Kimono.Core.Internal.MethodInvokers
{
    internal sealed class Action2ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?>>
    {
        public Action2ObjectMethodInvoker(Func<IInvocation, Action<object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0]);

            return null;
        }
    }
}
