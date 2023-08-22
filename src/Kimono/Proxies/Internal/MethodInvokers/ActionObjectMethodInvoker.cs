using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class ActionObjectMethodInvoker : MethodDelegateInvoker<Action<object?>>
    {
        public ActionObjectMethodInvoker(Func<Action<object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target);

            return null;
        }
    }
}
