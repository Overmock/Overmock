using System;

namespace Kimono.Core.Internal.MethodInvokers
{
    internal sealed class Action5ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?, object?>>
    {
        public Action5ObjectMethodInvoker(Func<IInvocation, Action<object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3]);

            return null;
        }
    }
}
