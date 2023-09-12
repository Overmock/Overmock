using System;
using System.Reflection;

namespace Kimono.Core.Delegates.Invokers
{
    internal sealed class MethodInfoDelegateInvoker : MethodDelegateInvoker<Func<object?, object?[], object?>>
    {
        public MethodInfoDelegateInvoker(MethodInfo methodInfo) : this(c => methodInfo.Invoke)
        {
        }

        public MethodInfoDelegateInvoker(Func<IInvocation, Func<object?, object?[], object?>> methodInvoke) : base(methodInvoke)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters);
        }
    }
}
