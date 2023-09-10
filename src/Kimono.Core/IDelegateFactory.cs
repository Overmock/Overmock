using Kimono.Delegates;
using Kimono.Emit;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Core
{
    /// <summary>
    /// When implemented, creates compiled delegates.
    /// </summary>
    public interface IDelegateFactory
    {
        /// <summary>
        /// Creates a delegate of the type <typeparamref name="TDelegate"/>.
        /// </summary>
        /// <typeparam name="TDelegate"></typeparam>
        /// <param name="metadata"></param>
        /// <param name="invocation"></param>
        /// <returns></returns>
        TDelegate CreateDelegate<TDelegate>(MethodMetadata metadata, IInvocation invocation) where TDelegate : Delegate;

        /// <summary>
        /// Generates the delegate invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters"></param>
        /// <returns>IMethodDelegateInvoker.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDelegateInvoker CreateDelegateInvoker(MethodMetadata metadata);

        /// <summary>
        /// Creates the action invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateActionInvoker(MethodMetadata metadata);

        /// <summary>
        /// Creates the function invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateFunctionInvoker(MethodMetadata metadata);
        
        public DynamicMethod CreateDynamicMethod(MethodMetadata metadata);

        void EmitProxyMethod(IEmitter emitter, MethodId methodId, MethodMetadata metadata);

        void EmitProxyDisposeMethod(IEmitter emitter, MethodInfo handleDisposeMethod);

        void EmitProxyConstructor(IEmitter emitter, ConstructorInfo baseConstructor);

        Func<IInterceptor<T>, T> CreateProxyConstructorDelegate<T>(Type proxyType, Type targetType, ConstructorInfo proxyConstructor) where T : class;
    }
}