using Kimono.Delegates.Invokers;
using Kimono.Delegates;
using Kimono.Msil;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using System.Globalization;

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
        /// Emits IL code for a proxy method that intercepts calls and forwards them to the interceptor.
        /// </summary>
        /// <remarks>
        /// This method generates IL that:
        /// 1. Loads generic type parameters (if any) into local variables using ldtoken/GetTypeFromHandle
        /// 2. Packages method arguments into an object[] array, boxing value types as needed
        /// 3. Calls the ProxyBase.HandleMethodCall with the method ID, generic types array, and arguments array
        /// 4. Unboxes/casts the return value to the appropriate type
        ///
        /// For generic methods, it's critical to use the generic parameter types from the MethodBuilder
        /// (passed via genericParameterTypes) rather than the original method's types, as the ldtoken
        /// instruction requires types that are valid in the current IL context.
        /// </remarks>
        /// <param name="emitter">The IL emitter to write instructions to.</param>
        /// <param name="methodId">The unique identifier for this method in the proxy.</param>
        /// <param name="metadata">Metadata about the method being proxied.</param>
        /// <param name="genericParameterTypes">The generic parameter types from the MethodBuilder (for generic methods).</param>
        public void EmitProxyMethod(IEmitter emitter, MethodId methodId, MethodMetadata metadata, Type[]? genericParameterTypes = null)
        {
            var returnType = metadata.ReturnType;
            var returnIsNotVoid = returnType != Types.Void;

            var locals = EmitGenericParameters(emitter, metadata, genericParameterTypes);

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
        protected static MethodInfo PrepareGenericMethod(MethodMetadata metadata, Type[]? genericParameters)
        {
            if (metadata.TargetMethod.IsGenericMethod)
            {
                if (genericParameters is null)
                {
                    throw new ArgumentNullException(nameof(genericParameters));
                }

                return metadata.TargetMethod.MakeGenericMethod(genericParameters);
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
        
        /// <summary>
        /// Emits IL to load generic type parameters into local variables.
        /// </summary>
        /// <remarks>
        /// For generic methods like void Method&lt;T&gt;(), this generates IL to:
        /// 1. Declare a local variable of type System.Type for each generic parameter
        /// 2. Load each generic parameter's type using ldtoken and Type.GetTypeFromHandle
        /// 3. Store the type in the corresponding local variable
        ///
        /// These locals are then used by EmitGenericLocalFieldTypes to create a Type[] array
        /// that gets passed to the interceptor, allowing runtime inspection of the generic types.
        ///
        /// CRITICAL: Must use genericParameterTypes (from the MethodBuilder) rather than metadata.GenericParameters
        /// (from the original method) because ldtoken requires types that exist in the current IL context.
        /// Using the wrong types causes a BadImageException at runtime.
        /// </remarks>
        /// <param name="emitter">The IL emitter to write instructions to.</param>
        /// <param name="metadata">Metadata about the method being proxied.</param>
        /// <param name="genericParameterTypes">The generic parameter types from the MethodBuilder.</param>
        /// <returns>Array of local variables that hold the generic type instances.</returns>
        private static LocalBuilder[] EmitGenericParameters(IEmitter emitter, MethodMetadata metadata, Type[]? genericParameterTypes)
        {
            var method = metadata.TargetMethod;
            if (method.IsGenericMethod)
            {
                // Use the generic parameter types from the MethodBuilder if provided, otherwise fall back to metadata
                var arguments = genericParameterTypes ?? metadata.GenericParameters;
                var locals = new LocalBuilder[arguments.Length];

                // Declare local variables for each generic type parameter
                for (int i = 0; i < arguments.Length; i++)
                {
                    locals[i] = emitter.DeclareLocal(Types.Type);
                }

                emitter.Nop();

                // Load each generic type into its local variable
                // IL Pattern: ldtoken T, call Type.GetTypeFromHandle, stloc.N
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

        /// <summary>
        /// Emits IL to create a Type[] array containing the generic type parameters.
        /// </summary>
        /// <remarks>
        /// This method reads the generic types from local variables (created by EmitGenericParameters)
        /// and packages them into a Type[] array on the stack. This array is then passed to
        /// ProxyBase.HandleMethodCall so the interceptor can inspect the generic types at runtime.
        ///
        /// Generated IL pattern for a method with 2 generic parameters:
        /// <code>
        /// ldc.i4.2              // Push array length
        /// newarr System.Type    // Create array
        /// dup                   // Duplicate array reference
        /// ldc.i4.0              // Push index 0
        /// ldloc.0               // Load first generic type from local
        /// stelem.ref            // Store in array[0]
        /// dup                   // Duplicate array reference
        /// ldc.i4.1              // Push index 1
        /// ldloc.1               // Load second generic type from local
        /// stelem.ref            // Store in array[1]
        /// // Array reference remains on stack for HandleMethodCall
        /// </code>
        ///
        /// For non-generic methods, loads Type.EmptyTypes field instead.
        /// </remarks>
        /// <param name="emitter">The IL emitter to write instructions to.</param>
        /// <param name="metadata">Metadata about the method being proxied.</param>
        private static void EmitGenericLocalFieldTypes(IEmitter emitter, MethodMetadata metadata)
        {
            if (metadata.TargetMethod.IsGenericMethod)
            {
                var arguments = metadata.GenericParameters;

                // Create Type[] array and populate it with generic types from locals
                emitter.IlGenerator.Emit(OpCodes.Ldc_I4, arguments.Length);
                emitter.IlGenerator.Emit(OpCodes.Newarr, Types.Type);

                for (int i = 0; i < arguments.Length; i++)
                {
                    emitter.IlGenerator.Emit(OpCodes.Dup);           // Duplicate array reference
                    emitter.IlGenerator.Emit(OpCodes.Ldc_I4, i);     // Push array index
                    emitter.IlGenerator.Emit(OpCodes.Ldloc, i);      // Load Type from local variable
                    emitter.IlGenerator.Emit(OpCodes.Stelem_Ref);    // Store in array
                }

                return;
            }

            // For non-generic methods, use the empty array
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