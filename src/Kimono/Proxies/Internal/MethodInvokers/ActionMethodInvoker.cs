using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class ActionMethodInvoker : MethodDelegateInvoker<Action>
    {
        public ActionMethodInvoker(Func<IInvocationContext, Action> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod();

            return null;
        }
    }
}
