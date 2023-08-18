using Kimono.Internal.MethodInvokers;
using Kimono.Proxies;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    internal sealed class MethodDelegateGenerator : IMethodDelegateGenerator
    {
        public IMethodDelegateInvoker Generate(RuntimeContext context, MethodInfo method)
        {
            var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (method.ReturnType == Constants.VoidType)
            {
                return HandleAction(context, method, parameters);
            }

            return HandleFunction(context, method, parameters);
        }

        private IMethodDelegateInvoker HandleAction(RuntimeContext context, MethodInfo method, Type[] parameters)
        {
            if (parameters.Length == 0)
            {
                return new ActionMethodInvoker((Action)EmitMethodInvocation(method, Constants.ActionType, parameters));
            }

            if (parameters.Length == 1)
            {
                return new ActionObjectMethodInvoker((Action<object?>)EmitMethodInvocation(method, Constants.ActionObjectType, parameters));
            }

            if (parameters.Length == 2)
            {
                return new Action2ObjectMethodInvoker((Action<object?, object?>)EmitMethodInvocation(method, Constants.Action2ObjectType, parameters));
            }

            if (parameters.Length == 3)
            {
                return new Action3ObjectMethodInvoker((Action<object?, object?, object?>)EmitMethodInvocation(method, Constants.Action3ObjectType, parameters));
            }

            if (parameters.Length == 4)
            {
                return new Action4ObjectMethodInvoker((Action<object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Action4ObjectType, parameters));
            }

            // Fallback to the origonal MethodInfo approach
            return new MethodInfoDelegateInvoker(context.ProxiedMember.CreateDelegate());
        }

        private IMethodDelegateInvoker HandleFunction(RuntimeContext context, MethodInfo method, Type[] parameters)
        {
            if (parameters.Length == 0)
            {
                return new FuncReturnMethodInvoker((Func<object?>)EmitMethodInvocation(method, Constants.FuncObjectType, parameters));
            }

            if (parameters.Length == 1)
            {
                return new FuncObjectReturnMethodInvoker((Func<object?, object?>)EmitMethodInvocation(method, Constants.Func1ObjectType, parameters));
            }

            if (parameters.Length == 2)
            {
                return new Func2ObjectReturnMethodInvoker((Func<object?, object?, object?>)EmitMethodInvocation(method, Constants.Func2ObjectType, parameters));
            }

            if (parameters.Length == 3)
            {
                return new Func3ObjectReturnMethodInvoker((Func<object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func3ObjectType, parameters));
            }

            if (parameters.Length == 4)
            {
                return new Func4ObjectReturnMethodInvoker((Func<object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func4ObjectType, parameters));
            }

            // Fallback to the origonal MethodInfo approach
            return new MethodInfoDelegateInvoker(context.ProxiedMember.CreateDelegate());
        }

        public Delegate EmitMethodInvocation(MethodInfo method, Type delegateType, Type[] parameters)
        {
            var returnType = method.ReturnType;
            var returnsVoid = returnType == Constants.VoidType;

            var dynamicMethod = new DynamicMethod(
                Constants.KimonoDeleateTypeNameFormat.ApplyFormat(method.Name),
                returnsVoid ? Constants.VoidType : Constants.ObjectType,
                parameters.Length > 0 
                    ? parameters.Select(p => Constants.ObjectType).ToArray()
                    : null
            );

            var emitter = dynamicMethod.GetILGenerator();
            emitter.Emit(OpCodes.Ldarg_0); // target

            for (int i = 0; i < parameters.Length; i++)
            {
                emitter.Emit(OpCodes.Ldarg, i + 1);
            }

            emitter.Emit(OpCodes.Callvirt, method);
            emitter.Emit(OpCodes.Ret);

            return dynamicMethod.CreateDelegate(delegateType);
        }

        public void EmitDisposeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod)
        {
            var methodBuilder = context.TypeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);

            var emitter = methodBuilder.GetILGenerator();

            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Ldarg_0);
            emitter.EmitCall(OpCodes.Call, Constants.ProxyTypeHandleDisposeCallMethod, null);
            emitter.Emit(OpCodes.Nop);
            emitter.Emit(OpCodes.Ret);
        }

        public void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor)
        {
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Ldarg_2);
            ilGenerator.Emit(OpCodes.Call, baseConstructor);
            ilGenerator.Emit(OpCodes.Ret);
        }
    }
}
