using System.Reflection;

namespace Kimono
{
    internal interface IMethodDelegateGenerator
    {
        IMethodDelegateInvoker Generate(RuntimeContext context, MethodInfo method);

        //Delegate EmitMethodInvocation(MethodInfo method, Type delegateType, Type[] parameters);

        //void EmitDisposeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod);

        //void EmitMemberInvokeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod);

        //void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor);
    }
}