using Kimono.Delegates;
using System;
using System.Reflection;

namespace Kimono.Core
{
    internal sealed class Invocation : IInvocation
    {
        private readonly IDelegateInvoker _invoker;

        public Invocation(IDelegateInvoker invoker)
        {
            _invoker = invoker;
        }

        public object? ReturnValue { get; set; }

        public object? Target { get; internal set; }

        public Type[] GenericParameters { get; internal set; }

        public ParameterInfo[] ParameterTypes { get; internal set; }

        public object?[] Parameters { get; internal set; }

        public MethodInfo Method { get; internal set; }

        public Type ReturnType { get; internal set; }

        public bool IsProperty { get; internal set; }

        public void Invoke(bool setReturnValue = true)
        {
            var target = Target;
            
            if (target == null)
            {
                return;
            }

            var value = _invoker?.Invoke(Target, this, Parameters);

            if (setReturnValue)
            {
                ReturnValue = value;
            }
        }
    }
}