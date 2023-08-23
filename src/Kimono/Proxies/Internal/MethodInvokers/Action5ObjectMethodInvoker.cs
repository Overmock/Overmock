﻿using System;

namespace Kimono.Proxies.Internal.MethodInvokers
{
    internal sealed class Action5ObjectMethodInvoker : MethodDelegateInvoker<Action<object?, object?, object?, object?, object?>>
    {
        public Action5ObjectMethodInvoker(Func<Action<object?, object?, object?, object?, object?>> invokeMethod) : base(invokeMethod)
        {
        }

        public sealed override object? Invoke(object? target, params object?[] parameters)
        {
            InvokeMethod(target, parameters[0], parameters[1], parameters[2], parameters[3]);

            return null;
        }
    }
}