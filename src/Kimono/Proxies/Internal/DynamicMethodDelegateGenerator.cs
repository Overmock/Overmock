using Kimono.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Internal
{
    internal sealed class DynamicMethodDelegateGenerator : MethodDelegateGenerator
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

        private static void GenerateActionInvocation(MethodInfo method, IEmitter body, Type[] parameters)
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

        private static void GenerateFuncInvocation(MethodInfo method, IEmitter body, Type[] parameters)
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

        protected override TDelegate GenerateMethodInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters, bool returnsVoid = false)
        {
            var parameterArray = parameters.Select(p => p.Type).ToArray();
            var dynamicMethod = CreateDynamicMethod(method, parameterArray, returnsVoid);

            var emitter = dynamicMethod.GetEmitter();

            if (returnsVoid)
            {
                GenerateActionInvocation(method, emitter, parameterArray);
            }
            else
            {
                GenerateFuncInvocation(method, emitter, parameterArray);
            }

            return (TDelegate)dynamicMethod.CreateDelegate(delegateType);
        }
    }
}
