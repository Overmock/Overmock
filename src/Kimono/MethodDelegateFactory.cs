using Kimono.Emit;
using Kimono.Proxies.Internal.MethodInvokers;
using Kimono.Proxies;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// Class MethodDelegateGenerator.
    /// Implements the <see cref="IMethodDelegateFactory" />
    /// </summary>
    /// <seealso cref="IMethodDelegateFactory" />
    public abstract class MethodDelegateFactory : IMethodDelegateFactory
    {
        /// <summary>
        /// Emits the dispose interceptor.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="disposeMethod">The dispose method.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void EmitDisposeMethod(IProxyContextBuilder context, MethodInfo disposeMethod)
        {
            var methodBuilder = context.TypeBuilder.DefineMethod(disposeMethod.Name, disposeMethod.Attributes ^ MethodAttributes.Abstract);

            methodBuilder.GetEmitter()
                .Nop()
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
        /// <param name="context">The context.</param>
        /// <param name="method">The method.</param>
        /// <returns>IMethodDelegateInvoker.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IMethodDelegateInvoker CreateDelegateInvoker(RuntimeContext context, MethodInfo method)
        {
            var parameters = context.GetParameters();

            if (method.ReturnType == Constants.VoidType)
            {
                return HandleAction(context, method, parameters);
            }

            return HandleFunction(context, method, parameters);
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
        protected abstract TDelegate GenerateMethodInvoker<TDelegate>(MethodInfo method, Type delegateType, IReadOnlyList<RuntimeParameter> parameters, bool returnsVoid = false)
            where TDelegate : Delegate;

        private IMethodDelegateInvoker HandleAction(RuntimeContext context, MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (parameters.Count == 0)
            {
                return new ActionObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?>>(method, Constants.Action1ObjectType, parameters, true));
            }

            if (parameters.Count == 1)
            {
                return new Action2ObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?, object?>>(method, Constants.Action2ObjectType, parameters, true));
            }

            if (parameters.Count == 2)
            {
                return new Action3ObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?, object?, object?>>(method, Constants.Action3ObjectType, parameters, true));
            }

            if (parameters.Count == 3)
            {
                return new Action4ObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?, object?, object?, object?>>(method, Constants.Action4ObjectType, parameters, true));
            }

            if (parameters.Count == 4)
            {
                return new Action5ObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?, object?, object?, object?, object?>>(method, Constants.Action5ObjectType, parameters, true));
            }

            if (parameters.Count == 5)
            {
                return new Action6ObjectMethodInvoker(() => GenerateMethodInvoker<Action<object?, object?, object?, object?, object?, object?>>(method, Constants.Action6ObjectType, parameters, true));
            }

            // Fallback to MethodInfo
            return new MethodInfoDelegateInvoker(() => context.ProxiedMember.CreateDelegate());
        }

        private IMethodDelegateInvoker HandleFunction(RuntimeContext context, MethodInfo method, IReadOnlyList<RuntimeParameter> parameters)
        {
            if (parameters.Count == 0)
            {
                return new FuncObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?>>(method, Constants.Func1ObjectType, parameters));
            }

            if (parameters.Count == 1)
            {
                return new Func2ObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?, object?>>(method, Constants.Func2ObjectType, parameters));
            }

            if (parameters.Count == 2)
            {
                return new Func3ObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?, object?, object?>>(method, Constants.Func3ObjectType, parameters));
            }

            if (parameters.Count == 3)
            {
                return new Func4ObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?, object?, object?, object?>>(method, Constants.Func4ObjectType, parameters));
            }

            if (parameters.Count == 4)
            {
                return new Func5ObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?, object?, object?, object?, object?>>(method, Constants.Func5ObjectType, parameters));
            }

            if (parameters.Count == 5)
            {
                return new Func6ObjectReturnMethodInvoker(() => GenerateMethodInvoker<Func<object?, object?, object?, object?, object?, object?, object?>>(method, Constants.Func6ObjectType, parameters));
            }

            // Fallback to MethodInfo
            return new MethodInfoDelegateInvoker(() => context.ProxiedMember.CreateDelegate());
        }
    }
}
