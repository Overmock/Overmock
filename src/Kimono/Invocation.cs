using Kimono.Delegates;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Kimono
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

        public Type[]? GenericParameters { get; internal set; }

        public ParameterInfo[]? ParameterTypes { get; internal set; }

        public object?[]? Parameters { get; internal set; }

        public MethodInfo? Method { get; internal set; }

        public Type? ReturnType { get; internal set; }

        public bool IsProperty { get; internal set; }

        public bool TargetInvoked { get; private set; }

        public T GetParameter<T>(string name)
        {
            ref var reference = ref MemoryMarshal.GetReference(ParameterTypes.AsSpan());

            for (int i = 0; i < ParameterTypes.Length; i++)
            {
                ref var param = ref Unsafe.Add(ref reference, i);

                if (param.Name == name)
                {
                    return (T)Parameters[i]!;
                }
            }

            throw new KeyNotFoundException(name);
        }

        public T GetParameter<T>(int index)
        {
            if (Parameters?.Length > index)
            {
                var parameter = Parameters[index];

                if (parameter is T value)
                {
                    return value;
                }
            }

            return default!;
        }

        public void Invoke(bool setReturnValue = true)
        {
            var target = Target;
            
            if (target == null)
            {
                return;
            }

            var value = _invoker?.Invoke(target, this, Parameters!);

            TargetInvoked = true;

            if (setReturnValue)
            {
                ReturnValue = value;
            }
        }
    }
}