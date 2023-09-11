using System;
using System.Reflection;

namespace Kimono.Core
{
    internal sealed class Invocation : IInvocation
    {
        public Type[] GenericParameters { get; internal set; }

        public ParameterInfo[] ParameterTypes { get; internal set; }

        public object?[] Parameters { get; internal set; }

        public MethodInfo Method { get; internal set; }

        public object? ReturnValue { get; set; }
    }
}