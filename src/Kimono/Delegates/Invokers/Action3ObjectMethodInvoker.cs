﻿using System;

namespace Kimono.Delegates.Invokers
{
    internal sealed class Action3ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?>>
    {
        public Action3ObjectMethodInvoker(Func<IInvocation, Action<object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1]);

            return null;
        }
    }
}
