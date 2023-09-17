using Kimono.Delegates.Invokers;
using Kimono.Delegates;
using Kimono.Msil;
using System;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Globalization;
using Microsoft.VisualBasic;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public class DelegateFactory : IDelegateFactory
    {
        private static IDelegateFactory _current = new DynamicMethodDelegateFactory();

        /// <summary>
        /// 
        /// </summary>
        public DelegateFactory()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static IDelegateFactory Current => _current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        public static IDelegateFactory Use(IDelegateFactory factory)
        {
            return Interlocked.Exchange(ref _current, factory);
        }

        /// <summary>
        /// Creates a delegate of the type <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        public TDelegate CreateDelegate<TDelegate>(MethodMetadata metadata, IInvocation invocation) where TDelegate : Delegate
        {
            if (metadata.TargetMethod.ReturnType == Types.Void)
            {
                return CreateActionInvoker<TDelegate>(metadata, typeof(TDelegate), invocation);
            }

            return CreateFuncInvoker<TDelegate>(metadata, typeof(TDelegate), invocation);
        }

        /// <summary>
        /// Generates the delegate invoker.
        /// </summary>
        /// <param name="metadata">The method metadata.</param>
        /// <returns>IMethodDelegateInvoker.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDelegateInvoker CreateDelegateInvoker(MethodMetadata metadata)
        {
            if (metadata.TargetMethod.ReturnType == Types.Void)
            {
                return CreateActionInvoker(metadata);
            }

            return CreateFunctionInvoker(metadata);
        }

        /// <summary>
        /// Creates the action invoker.
        /// </summary>
        /// <param name="metadata">The method metadata.</param>
        /// <returns></returns>
        public IDelegateInvoker CreateActionInvoker(MethodMetadata metadata)
        {
            var parameters = metadata.Parameters;

            if (parameters.Length == 0)
            {
                return new ActionObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?>>(metadata, Types.Action1ObjectType, invocation)
                );
            }

            if (parameters.Length == 1)
            {
                return new Action2ObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?, object?>>(metadata, Types.Action2ObjectType, invocation)
                );
            }

            if (parameters.Length == 2)
            {
                return new Action3ObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?, object?, object?>>(metadata, Types.Action3ObjectType, invocation)
                );
            }

            if (parameters.Length == 3)
            {
                return new Action4ObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?, object?, object?, object?>>(metadata, Types.Action4ObjectType, invocation)
                );
            }

            if (parameters.Length == 4)
            {
                return new Action5ObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?, object?, object?, object?, object?>>(metadata, Types.Action5ObjectType, invocation)
                );
            }

            if (parameters.Length == 5)
            {
                return new Action6ObjectMethodInvoker(invocation =>
                    CreateActionInvoker<Action<object?, object?, object?, object?, object?, object?>>(metadata, Types.Action6ObjectType, invocation)
                );
            }

            return new MethodInfoDelegateInvoker(metadata.TargetMethod);
        }

        /// <summary>
        /// Creates the function invoker.
        /// </summary>
        /// <param name="metadata">The method metadata.</param>
        /// <returns></returns>
        public IDelegateInvoker CreateFunctionInvoker(MethodMetadata metadata)
        {
            var parameters = metadata.Parameters;

            if (parameters.Length == 0)
            {
                return new FuncObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?>>(metadata, Types.Func1ObjectType, invocation)
                );
            }

            if (parameters.Length == 1)
            {
                return new Func2ObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?, object?>>(metadata, Types.Func2ObjectType, invocation)
                );
            }

            if (parameters.Length == 2)
            {
                return new Func3ObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?>>(metadata, Types.Func3ObjectType, invocation)
                );
            }

            if (parameters.Length == 3)
            {
                return new Func4ObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?>>(metadata, Types.Func4ObjectType, invocation)
                );
            }

            if (parameters.Length == 4)
            {
                return new Func5ObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?, object?>>(metadata, Types.Func5ObjectType, invocation)
                );
            }

            if (parameters.Length == 5)
            {
                return new Func6ObjectReturnMethodInvoker(invocation =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?, object?, object?>>(metadata, Types.Func6ObjectType, invocation)
                );
            }

            return new MethodInfoDelegateInvoker(metadata.TargetMethod);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="methodId"></param>
        /// <param name="metadata"></param>
        public void EmitProxyMethod(IEmitter emitter, MethodId methodId, MethodMetadata metadata)
        {
            var returnType = metadata.ReturnType;
            var returnIsNotVoid = returnType != Types.Void;

            var locals = EmitGenericParameters(emitter, metadata);

            if (returnIsNotVoid)
            {
                emitter.DeclareLocal(returnType);
            }

            var returnLabel = emitter.IlGenerator.DefineLabel();

            emitter.IlGenerator.Emit(OpCodes.Ldarg_0);
            emitter.IlGenerator.Emit(OpCodes.Ldc_I4, methodId.Current);

            EmitGenericLocalFieldTypes(emitter, metadata);

            if (metadata.Parameters.Length > 0)
            {
                emitter.IlGenerator.Emit(OpCodes.Ldc_I4, metadata.Parameters.Length);
                emitter.IlGenerator.Emit(OpCodes.Newarr, Types.Object);

                for (int i = 0; i < metadata.Parameters.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Dup);
                    emitter.IlGenerator.Emit(OpCodes.Ldc_I4, i);
                    emitter.IlGenerator.Emit(OpCodes.Ldarg, i + 1);

                    if (metadata.ParameterTypes[i].IsValueType)
                    {
                        emitter.IlGenerator.Emit(OpCodes.Box, metadata.ParameterTypes[i]);
                    }

                    emitter.IlGenerator.Emit(OpCodes.Stelem_Ref);
                }
            }
            else
            {
                emitter.IlGenerator.EmitCall(OpCodes.Call, Methods.EmptyObjectArray, null);
            }

            emitter.IlGenerator.Emit(OpCodes.Callvirt, Methods.HandleMethodCall);

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="targetType"></param>
        public void EmitProxyDisposeMethod(IEmitter emitter)
        {
            emitter.Nop().Load(0)
                .Invoke(Methods.HandleDisposeCall)
                .Nop().Ret();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="baseConstructor"></param>
        public void EmitProxyConstructor(IEmitter emitter, ConstructorInfo baseConstructor)
        {
            emitter.Load(0).Load(1)
                .BaseCtor(baseConstructor)
                .Ret();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyType"></param>
        /// <param name="targetType"></param>
        /// <param name="proxyConstructor"></param>
        /// <returns></returns>
        public Func<IInterceptor, T> CreateProxyConstructorDelegate<T>(Type proxyType, Type targetType, ConstructorInfo proxyConstructor) where T : class
        {
            var dynamicMethod = new DynamicMethod(
                string.Format(CultureInfo.CurrentCulture, Names.DynamicMethodName, proxyType.Name),
                targetType,
                Types.ProxyBaseCtorParameterTypes,
                proxyType
            );

            var iLGenerator = dynamicMethod.GetILGenerator();
            iLGenerator.Emit(OpCodes.Ldarg_0);
            iLGenerator.Emit(OpCodes.Newobj, proxyConstructor);
            iLGenerator.Emit(OpCodes.Ret);

            return (Func<IInterceptor, T>)dynamicMethod.CreateDelegate(
                Types.GetFuncProxyContextIInterceptorTType<T>()    
            );
        }

        /// <summary>
        /// Prepares the current method with the provided generic parameters.
        /// </summary>
        /// <param name="metadata"></param>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        protected static MethodInfo PrepareGenericMethod(MethodMetadata metadata, Type[] genericParameters)
        {
            if (metadata.TargetMethod.IsGenericMethod)
            {
                var genericMethod = metadata.TargetMethod.MakeGenericMethod(genericParameters);
                return genericMethod;
            }

            return metadata.TargetMethod;
        }

        /// <summary>
        /// Generates the method invoker.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="metadata">The method.</param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="invocation">The invocation.</param>
        /// <returns>TDelegate.</returns>
        protected virtual TDelegate CreateActionInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation invocation)
            where TDelegate : Delegate
        {
            return (TDelegate)metadata.TargetMethod.CreateDelegate(typeof(TDelegate));
        }

        /// <summary>
        /// Generates the method invoker.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="metadata">The method metadata.</param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="invocation">The invocation.</param>
        /// <returns>TDelegate.</returns>
        protected virtual TDelegate CreateFuncInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation invocation)
            where TDelegate : Delegate
        {
            return (TDelegate)metadata.TargetMethod.CreateDelegate(typeof(TDelegate));
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="metadata"></param>
        ///// <returns></returns>
        //public DynamicMethod CreateDynamicMethod(MethodMetadata metadata)
        //{
        //    return new DynamicMethod(
        //        Names.DynamicMethodName,
        //        Types.Object,
        //        metadata.Parameters.Select(p => Types.Object)
        //            .Concat(Types.SingleObjectArray)
        //            .ToArray()
        //    );
        //}
        
        private static LocalBuilder[] EmitGenericParameters(IEmitter emitter, MethodMetadata metadata)
        {
            var method = metadata.TargetMethod;
            if (method.IsGenericMethod)
            {
                var arguments = metadata.GenericParameters;
                var locals = new LocalBuilder[arguments.Length];

                for (int i = 0; i < arguments.Length; i++)
                {
                    locals[i] = emitter.DeclareLocal(Types.Type);
                }

                emitter.Nop();

                for (int i = 0; i < arguments.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Ldtoken, arguments[i]);
                    emitter.IlGenerator.Emit(OpCodes.Call, Methods.GetTypeFromHandle);
                    emitter.IlGenerator.Emit(OpCodes.Stloc, i);
                }

                return locals;
            }

            return Array.Empty<LocalBuilder>();
        }

        private static void EmitGenericLocalFieldTypes(IEmitter emitter, MethodMetadata metadata)
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

            if (metadata.TargetMethod.IsGenericMethod)
            {
                var arguments = metadata.GenericParameters;

                emitter.IlGenerator.Emit(OpCodes.Ldc_I4, arguments.Length);
                emitter.IlGenerator.Emit(OpCodes.Newarr, Types.Type);

                for (int i = 0; i < arguments.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Dup);
                    emitter.IlGenerator.Emit(OpCodes.Ldc_I4, i);
                    emitter.IlGenerator.Emit(OpCodes.Ldloc, i);
                    emitter.IlGenerator.Emit(OpCodes.Stelem_Ref);
                }

                return;
            }

            emitter.IlGenerator.Emit(OpCodes.Ldsfld, Fields.EmptyTypes);
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected static class Names
        {
            /// <summary>
            /// 
            /// </summary>
            public const string DynamicMethodName = "KimonoDM_{0}";
        }

        private static class Fields
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly FieldInfo EmptyTypes = typeof(Type).GetField(nameof(Type.EmptyTypes), BindingFlags.Static | BindingFlags.Public)!;
        }

        /// <summary>
        /// 
        /// </summary>
        internal protected static class Methods
        {
            /// <summary>
            /// 
            /// </summary>
            public static readonly MethodInfo GetTypeFromHandle =
                Types.Type.GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public)!;

            /// <summary>
            /// The empty object array method
            /// </summary>
            public static readonly MethodInfo EmptyObjectArray =
                Types.Array.GetMethod("Empty", BindingFlags.Static | BindingFlags.Public)!
                    .MakeGenericMethod(Types.Object);

            /// <summary>
            /// Gets the proxy type handle method call method.
            /// </summary>
            /// <returns>MethodInfo.</returns>
            public static readonly MethodInfo HandleMethodCall =
                Types.ProxyBaseNonGeneric.GetMethod("HandleMethodCall", BindingFlags.Instance | BindingFlags.NonPublic)!;

            /// <summary>
            /// Gets the proxy type handle method call method.
            /// </summary>
            /// <returns>MethodInfo.</returns>
            public static readonly MethodInfo HandleDisposeCall =
                Types.ProxyBaseNonGeneric.GetMethod("HandleDisposeCall", BindingFlags.Instance | BindingFlags.NonPublic)!;
        }
    }
}