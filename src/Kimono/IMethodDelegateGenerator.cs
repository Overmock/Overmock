using Kimono.Proxies;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono
{
    internal interface IMethodDelegateGenerator
    {
        IMethodDelegateInvoker Generate(RuntimeContext context, MethodInfo method);

        Delegate EmitMethodInvocation(MethodInfo method, Type delegateType, Type[] parameters, bool returnsVoid = false);

        void EmitDisposeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod);

        void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor);
    }
}