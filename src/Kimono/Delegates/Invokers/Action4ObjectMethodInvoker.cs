﻿using System;

namespace Kimono.Delegates.Invokers
{
    internal sealed class Action4ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?>>
    {
        public Action4ObjectMethodInvoker(Func<IInvocation, Action<object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2]);

            return null;
        }
    }
}
