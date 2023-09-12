using Kimono.Emit;
using Kimono.Proxies.Internal.MethodInvokers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// Class MethodDelegateGenerator.
    /// Implements the <see cref="IDelegateFactory" />
    /// </summary>
    /// <seealso cref="IDelegateFactory" />
    public abstract class DelegateFactory : IDelegateFactory
    {
        /// <summary>
        /// Emits the dispose interceptor.
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="disposeMethod">The dispose method.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void EmitDisposeDelegate(IEmitter emitter, MethodInfo disposeMethod)
        {
            emitter.Nop().Load(0)
                .Invoke(Constants.ProxyTypeHandleDisposeCallMethod)
                .Nop().Ret();
        }

        /// <summary>
        /// Emits the type initializer.
        /// </summary>
        /// <param name="emitter">The il generator.</param>
        /// <param name="baseConstructor">The base constructor.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void EmitConstructor(IEmitter emitter, ConstructorInfo baseConstructor)
        {
            emitter.Load(0).Load(1).Load(2)
                .BaseCtor(baseConstructor)
                .Ret();
        }

        /// <summary>
        /// Generates the delegate invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters"></param>
        /// <returns>IMethodDelegateInvoker.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDelegateInvoker CreateDelegateInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (method.ReturnType == Constants.VoidType)
            {
                return CreateActionInvoker(method, parameters);
            }

            return CreateFunctionInvoker(method, parameters);
        }

        /// <summary>
        /// Creates the action invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateActionInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (parameters.Count == 0)
            {
                return new ActionObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?>>(method, c, Constants.Action1ObjectType, parameters));
            }

            if (parameters.Count == 1)
            {
                return new Action2ObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?, object?>>(method, c, Constants.Action2ObjectType, parameters)
                );
            }

            if (parameters.Count == 2)
            {
                return new Action3ObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?, object?, object?>>(method, c, Constants.Action3ObjectType, parameters)
                );
            }

            if (parameters.Count == 3)
            {
                return new Action4ObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?, object?, object?, object?>>(method, c, Constants.Action4ObjectType, parameters)
                );
            }

            if (parameters.Count == 4)
            {
                return new Action5ObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?, object?, object?, object?, object?>>(method, c, Constants.Action5ObjectType, parameters)
                );
            }

            if (parameters.Count == 5)
            {
                return new Action6ObjectMethodInvoker(c =>
                    CreateActionInvoker<Action<object?, object?, object?, object?, object?, object?>>(method, c, Constants.Action6ObjectType, parameters)
                );
            }

            return new MethodInfoDelegateInvoker(method);
        }

        /// <summary>
        /// Creates the function invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateFunctionInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (parameters.Count == 0)
            {
                return new FuncObjectReturnMethodInvoker(c =>
                    CreateFuncInvoker<Func<object?, object?>>(method, c, Constants.Func1ObjectType, parameters)
                );
            }

            if (parameters.Count == 1)
            {
                return new Func2ObjectReturnMethodInvoker(c => 
                    CreateFuncInvoker<Func<object?, object?, object?>>(method, c, Constants.Func2ObjectType, parameters)
                );
            }

            if (parameters.Count == 2)
            {
                return new Func3ObjectReturnMethodInvoker(c =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?>>(method, c, Constants.Func3ObjectType, parameters)
                );
            }

            if (parameters.Count == 3)
            {
                return new Func4ObjectReturnMethodInvoker(c =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?>>(method, c, Constants.Func4ObjectType, parameters)
                );
            }

            if (parameters.Count == 4)
            {
                return new Func5ObjectReturnMethodInvoker(c =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?, object?>>(method, c, Constants.Func5ObjectType, parameters)
                );
            }

            if (parameters.Count == 5)
            {
                return new Func6ObjectReturnMethodInvoker(c =>
                    CreateFuncInvoker<Func<object?, object?, object?, object?, object?, object?, object?>>(method, c, Constants.Func6ObjectType, parameters)
                );
            }

            return new MethodInfoDelegateInvoker(method);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodInfo"></param>
        /// <param name="genericParameters"></param>
        /// <returns></returns>
        protected static MethodInfo PrepareGenericMethod(MethodInfo methodInfo, Type[] genericParameters)
        {
            if (methodInfo.IsGenericMethod)
            {
                var genericMethod = methodInfo.MakeGenericMethod(genericParameters);
                methodInfo = genericMethod;
            }

            return methodInfo;
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
        protected abstract TDelegate CreateActionInvoker<TDelegate>(MethodInfo method, IInvocationContext context, Type delegateType, IReadOnlyList<RuntimeParameter> parameters)
            where TDelegate : Delegate;

        /// <summary>
        /// Generates the method invoker.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="context"></param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>TDelegate.</returns>
        protected abstract TDelegate CreateFuncInvoker<TDelegate>(MethodInfo method, IInvocationContext context, Type delegateType, IReadOnlyList<RuntimeParameter> parameters)
            where TDelegate : Delegate;
    }
}
