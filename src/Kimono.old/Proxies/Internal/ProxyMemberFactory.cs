using Kimono.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Proxies.Internal
{
    internal class ProxyMemberFactory
    {
        private readonly IDelegateFactory? _delegateDelegateGenerator;

        public ProxyMemberFactory(IDelegateFactory? delegateDelegateGenerator = null)
        {
            _delegateDelegateGenerator = delegateDelegateGenerator;
        }

        protected IDelegateFactory DelegateFactory => _delegateDelegateGenerator ?? FactoryProvider.DelegateFactory;

        /// <summary>
        /// Creates the method.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="parameters"></param>
        /// <param name="methodId">The method identifier.</param>
        /// <returns>MethodBuilder.</returns>
        protected static MethodBuilder CreateMethod(IProxyContextBuilder context, MethodInfo methodInfo, IReadOnlyList<RuntimeParameter> parameters, int methodId)
        {
            var parameterTypes = parameters.Select(p => p.Type).ToArray();
            var methodBuilder = context.TypeBuilder.DefineMethod(
                methodInfo.Name,
                methodInfo.IsAbstract
                    ? methodInfo.Attributes ^ MethodAttributes.Abstract
                    : methodInfo.Attributes,
                methodInfo.ReturnType,
                parameterTypes
            );

            if (methodInfo.DeclaringType == Constants.ObjectType)
            {
                context.TypeBuilder.DefineMethodOverride(methodBuilder, methodInfo);
            }

            var emitter = methodBuilder.GetEmitter();
            var returnType = methodInfo.ReturnType;

            if (methodInfo.IsGenericMethod)
            {
                DefineGenericParameters(methodInfo, methodBuilder);
            }

            EmitMethodBody(methodInfo, emitter, returnType, parameterTypes, methodId);

            return methodBuilder;
        }

        /// <summary>
        /// Emits the method body.
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="emitter">The emitter.</param>
        /// <param name="returnType">Type of the return.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="methodId">The method identifier.</param>
        protected static void EmitMethodBody(MethodInfo methodInfo, IEmitter emitter, Type returnType, Type[] parameters, int methodId)
        {
            var returnIsNotVoid = returnType != Constants.VoidType;

            var locals = EmitGenericParameters(emitter, methodInfo);

            if (returnIsNotVoid)
            {
                emitter.DeclareLocal(returnType);
            }

            var returnLabel = emitter.IlGenerator.DefineLabel();

            //if (locals.Length == 0)
            //{
            //    emitter.Nop();
            //}
            
            emitter.IlGenerator.Emit(OpCodes.Ldarg_0);
            emitter.IlGenerator.Emit(OpCodes.Ldc_I4, methodId);

            EmitGenericLocalFieldTypes(emitter, methodInfo);

            if (parameters.Length > 0)
            {
                emitter.IlGenerator.Emit(OpCodes.Ldc_I4, parameters.Length);
                emitter.IlGenerator.Emit(OpCodes.Newarr, Constants.ObjectType);

                for (int i = 0; i < parameters.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Dup);
                    emitter.IlGenerator.Emit(OpCodes.Ldc_I4, i);
                    emitter.IlGenerator.Emit(OpCodes.Ldarg, i + 1);

                    if (parameters[i].IsValueType)
                    {
                        emitter.IlGenerator.Emit(OpCodes.Box, parameters[i]);
                    }

                    emitter.IlGenerator.Emit(OpCodes.Stelem_Ref);
                }
            }
            else
            {
                emitter.IlGenerator.EmitCall(OpCodes.Call, Constants.EmptyObjectArrayMethod, null);
            }

            emitter.IlGenerator.Emit(OpCodes.Callvirt, Constants.ProxyTypeHandleMethodCallMethod);

            if (returnIsNotVoid)
            {
                if (returnType.IsValueType)
                {
                    emitter.IlGenerator.Emit(OpCodes.Unbox_Any, returnType);
                }
                else
                {
                    if (returnType.IsGenericParameter)
                    {
                        emitter.IlGenerator.Emit(OpCodes.Unbox_Any, returnType);
                    }
                    else
                    {
                        emitter.IlGenerator.Emit(OpCodes.Castclass, returnType);
                    }

                    emitter.IlGenerator.Emit(OpCodes.Stloc, locals.Length);
                    emitter.IlGenerator.Emit(OpCodes.Br_S, returnLabel);

                    emitter.IlGenerator.MarkLabel(returnLabel);
                    emitter.IlGenerator.Emit(OpCodes.Ldloc, locals.Length);
                }
            }
            else
            {
                emitter.Emit(OpCodes.Pop);
            }

            emitter.Emit(OpCodes.Ret);
        }

        protected static LocalBuilder[] EmitGenericParameters(IEmitter emitter, MethodInfo methodInfo)
        {
            /*
            IL_0000: nop
            IL_0001: ldtoken !!T
            IL_0006: call class [System.Runtime]System.Type [System.Runtime]System.Type::GetTypeFromHandle(valuetype [System.Runtime]System.RuntimeTypeHandle)
            IL_000b: stloc.0
            IL_000c: ldtoken !!T1
            IL_0011: call class [System.Runtime]System.Type [System.Runtime]System.Type::GetTypeFromHandle(valuetype [System.Runtime]System.RuntimeTypeHandle)
            IL_0016: stloc.1
            IL_0017: ldtoken !!T2
            IL_001c: call class [System.Runtime]System.Type [System.Runtime]System.Type::GetTypeFromHandle(valuetype [System.Runtime]System.RuntimeTypeHandle)
            IL_0021: stloc.2
            IL_0022: ldtoken !!T3
            IL_0027: call class [System.Runtime]System.Type [System.Runtime]System.Type::GetTypeFromHandle(valuetype [System.Runtime]System.RuntimeTypeHandle)
            IL_002c: stloc.3
             */

            if (methodInfo.IsGenericMethod)
            {
                var arguments = methodInfo.GetGenericArguments();
                var locals = new LocalBuilder[arguments.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    locals[i] = emitter.DeclareLocal(Constants.TypeType);
                }

                emitter.Nop();

                for (int i = 0; i < arguments.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Ldtoken, arguments[i]);
                    emitter.IlGenerator.Emit(OpCodes.Call, Constants.GetTypeFromHandleMethod);
                    emitter.IlGenerator.Emit(OpCodes.Stloc, i);
                }

                return locals;
            }

            return Array.Empty<LocalBuilder>();
        }

        private static void EmitGenericLocalFieldTypes(IEmitter emitter, MethodInfo methodInfo)
        {
            /*
            IL_0030: ldc.i4.4
            IL_0031: newarr [System.Runtime]System.Type

            IL_0036: dup
            IL_0037: ldc.i4.0
            IL_0038: ldloc.0
            IL_0039: stelem.ref

            IL_003a: dup
            IL_003b: ldc.i4.1
            IL_003c: ldloc.1
            IL_003d: stelem.ref

            IL_003e: dup
            IL_003f: ldc.i4.2
            IL_0040: ldloc.2
            IL_0041: stelem.ref

            IL_0042: dup
            IL_0043: ldc.i4.3
            IL_0044: ldloc.3
            IL_0045: stelem.ref
             */

            if (methodInfo.IsGenericMethod)
            {
                var arguments = methodInfo.GetGenericArguments();

                emitter.IlGenerator.Emit(OpCodes.Ldc_I4, arguments.Length);
                emitter.IlGenerator.Emit(OpCodes.Newarr, Constants.TypeType);

                for (int i = 0; i < arguments.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Dup);
                    emitter.IlGenerator.Emit(OpCodes.Ldc_I4, i);
                    emitter.IlGenerator.Emit(OpCodes.Ldloc, i);
                    emitter.IlGenerator.Emit(OpCodes.Stelem_Ref);
                }

                return;
            }

            emitter.IlGenerator.Emit(OpCodes.Ldsfld, Constants.EmptyTypesField);
        }

        /// <summary>
        /// Defines the generic parameters.
        /// </summary>
        /// <param name="methodInfo">The method information.</param>
        /// <param name="methodBuilder">The method builder.</param>
        protected static void DefineGenericParameters(MethodInfo methodInfo, MethodBuilder methodBuilder)
        {
            var genericParameters = methodInfo.GetGenericArguments();
            var genericParameterBuilders = methodBuilder.DefineGenericParameters(genericParameters.Select(t => t.Name).ToArray());

            for (int i = 0; i < genericParameterBuilders.Length; i++)
            {
                var baseGenericArgument = genericParameters[i];
                var genericParameterBuilder = genericParameterBuilders[i];

                genericParameterBuilder.SetGenericParameterAttributes(baseGenericArgument.GenericParameterAttributes);

                foreach (var baseTypeConstraint in baseGenericArgument.GetGenericParameterConstraints())
                {
                    if (baseTypeConstraint.IsInterface)
                    {
                        genericParameterBuilder.SetInterfaceConstraints(baseTypeConstraint);
                    }
                    else
                    {
                        genericParameterBuilder.SetBaseTypeConstraint(baseTypeConstraint);
                    }
                }
            }
        }
    }
}
