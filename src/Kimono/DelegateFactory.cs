﻿using Kimono.Emit;
using Kimono.Proxies;
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
            emitter.Nop()
                .Load(0)
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
                return new ActionObjectMethodInvoker(() => CreateMethodInvoker<Action<object?>>(method, Constants.Action1ObjectType, parameters, true));
            }

            if (parameters.Count == 1)
            {
                return new Action2ObjectMethodInvoker(() => CreateMethodInvoker<Action<object?, object?>>(method, Constants.Action2ObjectType, parameters, true));
            }

            if (parameters.Count == 2)
            {
                return new Action3ObjectMethodInvoker(() => CreateMethodInvoker<Action<object?, object?, object?>>(method, Constants.Action3ObjectType, parameters, true));
            }

            if (parameters.Count == 3)
            {
                return new Action4ObjectMethodInvoker(() => CreateMethodInvoker<Action<object?, object?, object?, object?>>(method, Constants.Action4ObjectType, parameters, true));
            }

            if (parameters.Count == 4)
            {
                return new Action5ObjectMethodInvoker(() => CreateMethodInvoker<Action<object?, object?, object?, object?, object?>>(method, Constants.Action5ObjectType, parameters, true));
            }

            if (parameters.Count == 5)
            {
                return new Action6ObjectMethodInvoker(() => CreateMethodInvoker<Action<object?, object?, object?, object?, object?, object?>>(method, Constants.Action6ObjectType, parameters, true));
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
                return new FuncObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?>>(method, Constants.Func1ObjectType, parameters));
            }

            if (parameters.Count == 1)
            {
                return new Func2ObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?, object?>>(method, Constants.Func2ObjectType, parameters));
            }

            if (parameters.Count == 2)
            {
                return new Func3ObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?, object?, object?>>(method, Constants.Func3ObjectType, parameters));
            }

            if (parameters.Count == 3)
            {
                return new Func4ObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?, object?, object?, object?>>(method, Constants.Func4ObjectType, parameters));
            }

            if (parameters.Count == 4)
            {
                return new Func5ObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?, object?, object?, object?, object?>>(method, Constants.Func5ObjectType, parameters));
            }

            if (parameters.Count == 5)
            {
                return new Func6ObjectReturnMethodInvoker(() => CreateMethodInvoker<Func<object?, object?, object?, object?, object?, object?, object?>>(method, Constants.Func6ObjectType, parameters));
            }

            return new MethodInfoDelegateInvoker(method);
        }

        /// <summary>
        /// Generates the method invoker.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="method">The method.</param>
        /// <param name="delegateType">Type of the delegate.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="returnsVoid">if set to <c>true</c> [returns void].</param>
        /// <returns>TDelegate.</returns>
        protected abstract TDelegate CreateMethodInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters, bool returnsVoid = false)
            where TDelegate : Delegate;

    }
}