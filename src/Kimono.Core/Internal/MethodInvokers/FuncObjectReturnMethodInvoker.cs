﻿using System;

namespace Kimono.Core.Internal.MethodInvokers
{
    internal sealed class FuncObjectReturnMethodInvoker : MethodDelegateInvoker<Func<object?, object?>>
    {
        public FuncObjectReturnMethodInvoker(Func<IInvocation, Func<object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            return InvokeMethod(target);
        }
    }
}
