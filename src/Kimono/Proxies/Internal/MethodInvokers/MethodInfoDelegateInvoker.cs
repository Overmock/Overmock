using System;
using System.Reflection;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class MethodInfoDelegateInvoker : MethodDelegateInvoker<Func<object?, object?[], object?>>
    {
        public MethodInfoDelegateInvoker(MethodInfo methodInfo) : this(() => methodInfo.Invoke)
        {
        }

        public MethodInfoDelegateInvoker(Func<Func<object?, object?[], object?>> methodInvoke) : base(methodInvoke)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            return InvokeMethod(target, parameters);
        }
    }
}
