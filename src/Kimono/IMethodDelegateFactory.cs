using Kimono.Emit;
using Kimono.Proxies;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// Interface IMethodDelegateFactory
    /// </summary>
    public interface IMethodDelegateFactory
    {
        /// <summary>
        /// Creates the delegate invoker.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="method">The method.</param>
        /// <returns>IMethodDelegateInvoker.</returns>
        IMethodDelegateInvoker CreateDelegateInvoker(RuntimeContext context, MethodInfo method);

        /// <summary>
        /// Emits the dispose method.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="disposeMethod">The dispose method.</param>
        void EmitDisposeMethod(IProxyContextBuilder context, MethodInfo disposeMethod);

        /// <summary>
        /// Emits the constructor.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="baseConstructor">The base constructor.</param>
        void EmitConstructor(IEmitter emitter, ConstructorInfo baseConstructor);
    }
}