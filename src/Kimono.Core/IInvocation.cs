using System;
using System.Reflection;

namespace Kimono.Core
{
    public interface IInvocation
    {
        Type[] GenericParameters { get; }
        
        MethodInfo Method { get; }
    }
}