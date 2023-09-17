using Kimono;
using Kimono.Msil;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Delegates
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DynamicMethodDelegateFactory : DelegateFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="delegateType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override TDelegate CreateActionInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation context)
        {
            return CreateInvocation<TDelegate>(context, metadata, delegateType, Types.Void, metadata.Parameters, GenerateActionInvocation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="delegateType"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected override TDelegate CreateFuncInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation context)
        {
            return CreateInvocation<TDelegate>(context, metadata, delegateType, Types.Object, metadata.Parameters, GenerateFuncInvocation);
        }

        private static TDelegate CreateInvocation<TDelegate>(
            IInvocation invocation,
            MethodMetadata metadata,
            Type delegateType,
            Type returnType,
            ParameterInfo[] parameters,
            Action<MethodInfo, IEmitter, Type[]> resultEmitter) where TDelegate : Delegate
        {
            var methodInfo = PrepareGenericMethod(metadata, invocation.GenericParameters);

            var parameterArray = parameters.Select(p => p.ParameterType).ToArray();
            var dynamicMethod = CreateDynamicMethod(metadata.TargetMethod, parameterArray, returnType);

            var emitter = dynamicMethod.GetEmitter();

            if (returnType != Types.Void)
            {
                emitter.DeclareLocal(Types.Object);
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

                //if (parameterArray[i].IsGenericParameter)
                //{
                //    emitter.Emit(OpCodes.Constrained, parameterArray[i]);
                //}
            }

            emitter.Invoke(methodInfo);
            resultEmitter(methodInfo, emitter, parameterArray);
            emitter.Ret();

            return (TDelegate)dynamicMethod.CreateDelegate(delegateType);
        }

        private static DynamicMethod CreateDynamicMethod(MethodInfo method, Type[] parameters, Type returnType)
        {
            return new DynamicMethod(
                string.Format(Names.DynamicMethodName, method.Name),
                returnType, parameters.Length > 0
                    ? parameters.Select(p => Types.Object).Concat(new[] { Types.Object }).ToArray()
                    : new[] { Types.Object }
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
                emitter.CastTo(Types.Object);
            }

            emitter.Emit(OpCodes.Stloc_0)
                .Emit(OpCodes.Br_S, returnLabel)
                .Mark(returnLabel)
                .Emit(OpCodes.Ldloc_0);
        }
    }
}
