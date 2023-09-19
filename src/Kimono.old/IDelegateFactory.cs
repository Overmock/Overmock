using Kimono.Emit;
using System.Collections.Generic;
using System.Reflection;

namespace Kimono
{
    /// <summary>
    /// Interface IMethodDelegateFactory
    /// </summary>
    public interface IDelegateFactory
    {
        /// <summary>
        /// Creates the delegate invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters"></param>
        /// <returns>IMethodDelegateInvoker.</returns>
        IDelegateInvoker CreateDelegateInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters);

        /// <summary>
        /// Emits the dispose method.
        /// </summary>
        /// <param name="emitter"></param>
        /// <param name="disposeMethod">The dispose method.</param>
        void EmitDisposeDelegate(IEmitter emitter, MethodInfo disposeMethod);

        /// <summary>
        /// Emits the constructor.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <param name="baseConstructor">The base constructor.</param>
        void EmitConstructor(IEmitter emitter, ConstructorInfo baseConstructor);

        /// <summary>
        /// Creates the action invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        IDelegateInvoker? CreateActionInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters);

        /// <summary>
        /// Creates the function invoker.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Nullable&lt;IDelegateInvoker&gt;.</returns>
        IDelegateInvoker? CreateFunctionInvoker(MethodInfo method, IReadOnlyList<RuntimeParameter> parameters);
    }
}