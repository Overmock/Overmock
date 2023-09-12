using System;
using System.Reflection;

namespace Kimono.Core
{
    public interface IInvocation
    {
        Type[] GenericParameters { get; }

        ParameterInfo[] ParameterTypes { get; }

        object?[] Parameters { get; }
        
        MethodInfo Method { get; }

        object? ReturnValue { get; set; }

        bool IsProperty { get; }

        void Invoke(bool setReturnValue = true);
    }
}