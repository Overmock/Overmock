using System;

namespace Kimono.Core.Delegates.Invokers
{
    internal sealed class ActionMethodInvoker : MethodDelegateInvoker<Action>
    {
        public ActionMethodInvoker(Func<IInvocation, Action> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod();

            return null;
        }
    }
}
