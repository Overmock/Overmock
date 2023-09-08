using Kimono.Core.Internal.MethodInvokers;
using Kimono.Delegates;
using Kimono.Emit;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

namespace Kimono.Core
{
    public class DelegateFactory : IDelegateFactory
    {
        private static IDelegateFactory _current = new DynamicMethodDelegateFactory();

        public DelegateFactory()
        {
        }

        public static IDelegateFactory Current => _current;

        public static IDelegateFactory Use(IDelegateFactory factory)
        {
            return Interlocked.Exchange(ref _current, factory);
        }

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
        /// <param name="method">The method.</param>
        /// <param name="parameters"></param>
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
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
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
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
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
        /// Creates a 
        /// </summary>
        /// <param name="methodInfo"></param>
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
        /// <param name="method">The method.</param>
        /// <param name="context"></param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="parameters">The parameters.</param>
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
        /// <param name="method">The method.</param>
        /// <param name="context"></param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>TDelegate.</returns>
        protected virtual TDelegate CreateFuncInvoker<TDelegate>(MethodMetadata metadata, Type delegateType, IInvocation invocation)
            where TDelegate : Delegate
        {
            return (TDelegate)metadata.TargetMethod.CreateDelegate(typeof(TDelegate));
        }

        private DynamicMethod CreateDynamicMethod(MethodMetadata metadata)
        {
            var parameters = metadata.Parameters;
            return new DynamicMethod(
                Names.DynamicMethodName,
                Types.Object, parameters.Length > 0
                    ? parameters.Select(p => Types.Object)
                        .Concat(Types.SingleObjectArray)
                        .ToArray()
                    : Types.SingleObjectArray);


        }

        protected static class Names
        {
            public const string DynamicMethodName = "KimonoDM_{0}";
        }

        protected static class Types
        {
            public static readonly Type Object = typeof(object);
            public static readonly Type[] SingleObjectArray = new[] { Types.Object };
            public static readonly Type Void = typeof(void);
            public static readonly Type Type = typeof(Type);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action1ObjectType = typeof(Action<object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action2ObjectType = typeof(Action<object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action3ObjectType = typeof(Action<object?, object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action4ObjectType = typeof(Action<object?, object?, object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action5ObjectType = typeof(Action<object?, object?, object?, object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Action6ObjectType = typeof(Action<object?, object?, object?, object?, object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Func1ObjectType = typeof(Func<object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Func2ObjectType = typeof(Func<object?, object?, object?>);
            /// <summary>
            /// The action type taking one object.
            /// </summary>
            public static readonly Type Func3ObjectType = typeof(Func<object?, object?, object?, object?>);
            /// <summary>
            /// The func type taking 3 objects.
            /// </summary>
            public static readonly Type Func4ObjectType = typeof(Func<object?, object?, object?, object?, object?>);
            /// <summary>
            /// The func type taking 5 objects.
            /// </summary>
            public static readonly Type Func5ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?>);
            /// <summary>
            /// The func type taking 6 objects.
            /// </summary>
            public static readonly Type Func6ObjectType = typeof(Func<object?, object?, object?, object?, object?, object?, object?>);
            /// <summary>
            /// 
            /// </summary>
            public static readonly MethodInfo GetTypeFromHandleMethod = Type.GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public)!;
        }
    }
}