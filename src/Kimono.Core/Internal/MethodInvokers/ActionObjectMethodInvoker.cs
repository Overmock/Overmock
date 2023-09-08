﻿using System;

namespace Kimono.Core.Internal.MethodInvokers
{
    internal sealed class ActionObjectMethodInvoker : MethodDelegateInvoker<Action<object?>>
    {
        public ActionObjectMethodInvoker(Func<IInvocation, Action<object?>> invokeMethod) : base(invokeMethod)
        {
        }

        protected sealed override object? InvokeCore(object? target, params object?[] parameters)
        {
            InvokeMethod(target);

            return null;
        }
    }
}
