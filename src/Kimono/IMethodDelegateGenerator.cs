using Kimono.Emit;
using Kimono.Proxies;
using System.Reflection;

namespace Kimono
{
    internal interface IMethodDelegateGenerator
    {
        IMethodDelegateInvoker GenerateDelegateInvoker(RuntimeContext context, MethodInfo method);

        //Delegate EmitMethodInvocation(MethodInfo method, Type delegateType, Type[] parameters);

        void EmitDisposeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod);

        //void EmitMemberInvokeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod);

        void EmitTypeInitializer(IEmitter emitter, ConstructorInfo baseConstructor);
    }
}