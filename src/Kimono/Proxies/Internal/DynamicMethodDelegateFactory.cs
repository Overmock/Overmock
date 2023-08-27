﻿using Kimono.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Proxies.Internal
{
    internal sealed class DynamicMethodDelegateFactory : DelegateFactory
    {
        protected override TDelegate CreateActionInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters)
        {
            return CreateInvocation<TDelegate>(method, delegateType, Constants.VoidType, parameters, GenerateActionInvocation);
        }

        protected override TDelegate CreateFuncInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters)
        {
            return CreateInvocation<TDelegate>(method, delegateType, Constants.ObjectType, parameters, GenerateFuncInvocation);
        }

        private static TDelegate CreateInvocation<TDelegate>(
            MethodInfo methodInfo,
            Type delegateType,
            Type returnType,
            IReadOnlyList<RuntimeParameter> parameters,
            Action<MethodInfo, IEmitter, Type[]> resultEmitter) where TDelegate : Delegate
        {
            var parameterArray = parameters.Select(p => p.Type).ToArray();
            var dynamicMethod = CreateDynamicMethod(methodInfo, parameterArray, returnType);

            var emitter = dynamicMethod.GetEmitter();

            if (returnType != Constants.VoidType)
            {
                emitter.DeclareLocal(Constants.ObjectType);
            }

            emitter.Emit(OpCodes.Nop)
                .Emit(OpCodes.Ldarg_0) // target
                .CastTo(methodInfo.DeclaringType!);

            for (int i = 0; i < parameterArray.Length; i++)
            {
                emitter.Emit(OpCodes.Ldarg, i + 1);

                if (parameterArray[i].IsValueType)
                {
                    emitter.Emit(OpCodes.Unbox_Any, parameterArray[i]);
                }
            }

            emitter.Invoke(methodInfo);
            resultEmitter(methodInfo, emitter, parameterArray);
            emitter.Ret();

            return (TDelegate)dynamicMethod.CreateDelegate(delegateType);
        }

        private static DynamicMethod CreateDynamicMethod(MethodInfo method, Type[] parameters, Type returnType)
        {
            return new DynamicMethod(
                Constants.KimonoDelegateTypeNameFormat.ApplyFormat(method.Name),
                returnType, parameters.Length > 0
                    ? parameters.Select(p => Constants.ObjectType).Concat(new[] { Constants.ObjectType }).ToArray()
                    : new[] { Constants.ObjectType }
            );
        }

        private static void GenerateActionInvocation(MethodInfo method, IEmitter emitter, Type[] parameters)
        {
            emitter.Nop();
        }

        private static void GenerateFuncInvocation(MethodInfo method, IEmitter emitter, Type[] parameters)
        {
            var returnType = method.ReturnType;
            var returnLabel = emitter.Label();

            if (returnType.IsValueType)
            {
                emitter.Box(returnType);
            }
            else
            {
                emitter.CastTo(Constants.ObjectType);
            }

            emitter.Emit(OpCodes.Stloc_0)
                .Emit(OpCodes.Br_S, returnLabel)
                .Mark(returnLabel)
                .Emit(OpCodes.Ldloc_0);
        }
    }
}
