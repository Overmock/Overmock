using Kimono.Delegates;
using Kimono.Msil;
using System;
using System.Reflection;

namespace Kimono
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
        /// <param name="metadata"></param>
        /// <returns>IMethodDelegateInvoker.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public IDelegateInvoker CreateDelegateInvoker(MethodMetadata metadata);

        /// <summary>
        /// Creates the action invoker.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateActionInvoker(MethodMetadata metadata);

        /// <summary>
        /// Creates the function invoker.
        /// </summary>
        /// <param name="metadata"></param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        public IDelegateInvoker CreateFunctionInvoker(MethodMetadata metadata);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="methodId"></param>
        /// <param name="metadata"></param>
        void EmitProxyMethod(IEmitter emitter, MethodId methodId, MethodMetadata metadata);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="baseConstructor"></param>
        void EmitProxyConstructor(IEmitter emitter, ConstructorInfo baseConstructor);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emitter"></param>
        void EmitProxyDisposeMethod(IEmitter emitter);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="proxyType"></param>
        /// <param name="targetType"></param>
        /// <param name="proxyConstructor"></param>
        /// <returns></returns>
        Func<IInterceptor, T> CreateProxyConstructorDelegate<T>(Type proxyType, Type targetType, ConstructorInfo proxyConstructor) where T : class;
    }
}