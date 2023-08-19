using Kimono.Internal.MethodInvokers;
using Kimono.Proxies;
using Kimono.Emit;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    internal sealed class MethodDelegateGenerator : IMethodDelegateGenerator
    {
        private static DynamicMethod CreateDynamicMethod(MethodInfo method, Type[] parameters, bool returnsVoid)
        {
            return new DynamicMethod(
                Constants.KimonoDelegateTypeNameFormat.ApplyFormat(method.Name),
                returnsVoid ? Constants.VoidType : Constants.ObjectType,
                parameters.Length > 0
                    ? parameters.Select(p => Constants.ObjectType).Concat(new Type[] { Constants.ObjectType }).ToArray()
                    : Type.EmptyTypes,
                true
            );
        }

        private static void EmitActionMethodInvocation(MethodInfo method, IEmitter body, Type[] parameters)
        {
            // Load the first parameter (object instance)
            body.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < parameters.Length; i++)
            {
                body.Emit(OpCodes.Ldarg, i + 1);

                if (parameters[i].IsValueType)
                {
                    body.Emit(OpCodes.Unbox_Any, parameters[i]);
                }
            }

            // Call the method on the instance with parameters
            body.Emit(OpCodes.Callvirt, method);

            body.Pop();

            // Return the result
            body.Emit(OpCodes.Ret);
        }

        private static void EmitFuncMethodInvocation(MethodInfo method, IEmitter body, Type[] parameters)
        {
            var returnType = method.ReturnType;
            // Load the first parameter (object instance)
            body.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < parameters.Length; i++)
            {
                body.Emit(OpCodes.Ldarg, i + 1);

                if (parameters[i].IsValueType)
                {
                    body.Emit(OpCodes.Unbox, parameters[i]);
                }
            }

            // Call the method on the instance with parameters
            body.Emit(OpCodes.Callvirt, method);

            // Return the result
            body.Emit(OpCodes.Ret);

            //var emitter = dynamicMethod.GetILGenerator();
            ////emitter.Emit(OpCodes.Nop);

            //emitter.Emit(OpCodes.Ldarg_0); // target

            //for (int i = 0; i < parameters.Length; i++)
            //{
            //    emitter.Emit(OpCodes.Ldarg, i + 1);

            //    if (parameters[i].IsValueType)
            //    {
            //        emitter.Emit(OpCodes.Unbox_Any, parameters[i]);
            //    }
            //}

            //emitter.Emit(OpCodes.Callvirt, method);

            //if (returnType.IsValueType)
            //{
            //    emitter.Emit(OpCodes.Box, Constants.ObjectType);
            //}

            //if (returnsVoid)
            //{
            //    emitter.Emit(OpCodes.Pop);
            //}

            ////if (returnsVoid)
            ////{!returnsVoid &&
            ////    emitter.Emit(OpCodes.);
            ////}

            //emitter.Emit(OpCodes.Ret);
        }

        public IMethodDelegateInvoker Generate(RuntimeContext context, MethodInfo method)
        {
            var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();

            if (method.ReturnType == Constants.VoidType)
            {
                return HandleAction(context, method, parameters);
            }

            return HandleFunction(context, method, parameters);
        }

        public Delegate EmitMethodInvocation(MethodInfo method, Type delegateType, Type[] parameters, bool returnsVoid = false)
        {
            var dynamicMethod = CreateDynamicMethod(method, parameters, returnsVoid);

            var emitter = dynamicMethod.GetEmitter();

            if (returnsVoid)
            {
                EmitActionMethodInvocation(method, emitter, parameters);
            }
            else
            {
                EmitFuncMethodInvocation(method, emitter, parameters);
            }

            return dynamicMethod.CreateDelegate(delegateType);
        }

        public void EmitDisposeInterceptor(IProxyContextBuilder context, MethodInfo disposeMethod)
        {
            var methodBuilder = context.TypeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);

            methodBuilder.GetEmitter()
                .Nop().Load(0)
                .Invoke(Constants.ProxyTypeHandleDisposeCallMethod)
                .Nop().Ret();
        }

        public void EmitTypeInitializer(ILGenerator ilGenerator, ConstructorInfo baseConstructor)
        {
            Emitter.For(ilGenerator)
                .Load(0).Load(1).Load(2)
                .BaseCtor(baseConstructor)
                .Ret();
        }

        private IMethodDelegateInvoker HandleAction(RuntimeContext context, MethodInfo method, Type[] parameters)
        {
            if (parameters.Length == 0)
            {
                return new ActionObjectMethodInvoker(() => (Action<object?>)EmitMethodInvocation(method, Constants.Action1ObjectType, parameters, true));
            }

            if (parameters.Length == 1)
            {
                return new Action2ObjectMethodInvoker(() => (Action<object?, object?>)EmitMethodInvocation(method, Constants.Action2ObjectType, parameters, true));
            }

            if (parameters.Length == 2)
            {
                return new Action3ObjectMethodInvoker(() => (Action<object?, object?, object?>)EmitMethodInvocation(method, Constants.Action3ObjectType, parameters, true));
            }

            if (parameters.Length == 3)
            {
                return new Action4ObjectMethodInvoker(() => (Action<object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Action4ObjectType, parameters, true));
            }

            if (parameters.Length == 4)
            {
                return new Action5ObjectMethodInvoker(() => (Action<object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Action5ObjectType, parameters, true));
            }

            if (parameters.Length == 5)
            {
                return new Action6ObjectMethodInvoker(() => (Action<object?, object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Action6ObjectType, parameters, true));
            }

            // Fallback to MethodInfo
            return new MethodInfoDelegateInvoker(() => context.ProxiedMember.CreateDelegate());
        }

        private IMethodDelegateInvoker HandleFunction(RuntimeContext context, MethodInfo method, Type[] parameters)
        {
            if (parameters.Length == 0)
            {
                return new FuncObjectReturnMethodInvoker(() => (Func<object?, object?>)EmitMethodInvocation(method, Constants.Func1ObjectType, parameters));
            }

            if (parameters.Length == 1)
            {
                return new Func2ObjectReturnMethodInvoker(() => (Func<object?, object?, object?>)EmitMethodInvocation(method, Constants.Func2ObjectType, parameters));
            }

            if (parameters.Length == 2)
            {
                return new Func3ObjectReturnMethodInvoker(() => (Func<object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func3ObjectType, parameters));
            }

            if (parameters.Length == 3)
            {
                return new Func4ObjectReturnMethodInvoker(() => (Func<object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func4ObjectType, parameters));
            }

            if (parameters.Length == 4)
            {
                return new Func5ObjectReturnMethodInvoker(() => (Func<object?, object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func5ObjectType, parameters));
            }

            if (parameters.Length == 5)
            {
                return new Func6ObjectReturnMethodInvoker(() => (Func<object?, object?, object?, object?, object?, object?, object?>)EmitMethodInvocation(method, Constants.Func6ObjectType, parameters));
            }

            // Fallback to MethodInfo
            return new MethodInfoDelegateInvoker(() => context.ProxiedMember.CreateDelegate());
        }
    }
}
