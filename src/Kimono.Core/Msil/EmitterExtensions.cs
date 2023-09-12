using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Core.Msil
{
    /// <summary>
    /// Class EmitterExtensions.
    /// </summary>
    public static partial class EmitterExtensions
    {
        /// <summary>
        /// Gets the emitter for the MethodBuilder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IEmitter.</returns>
        public static IEmitter GetEmitter(this MethodBuilder builder) => Emitter.For(builder.GetILGenerator());

        /// <summary>
        /// Gets the emitter for the MethodBuilder.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns>IEmitter.</returns>
        public static IEmitter GetEmitter(this ConstructorBuilder builder) => Emitter.For(builder.GetILGenerator());

        /// <summary>
        /// Gets the emitter for the MethodBuilder.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>IEmitter.</returns>
        public static IEmitter GetEmitter(this DynamicMethod method) => Emitter.For(method.GetILGenerator());

        /// <summary>
        /// Nops the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <returns>IEmitter.</returns>
        public static IEmitter<TDelegate> Nop<TDelegate>(this IEmitter<TDelegate> emitter) where TDelegate : Delegate
        {
            emitter.Emit(OpCodes.Nop);
            return emitter;
        }

        /// <summary>
        /// Bases the ctor.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="constructor">The constructor.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> BaseCtor<TDelegate>(this IEmitter<TDelegate> emitter, ConstructorInfo constructor) where TDelegate : Delegate
        {
            emitter.Emit(OpCodes.Call, constructor);
            return emitter;
        }

        /// <summary>
        /// Creates new ctor.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="constructor">The constructor.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> NewCtor<TDelegate>(this IEmitter<TDelegate> emitter, ConstructorInfo constructor) where TDelegate : Delegate
        {
            emitter.Emit(OpCodes.Newobj, constructor);
            return emitter;
        }

        /// <summary>
        /// Emits the call.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="method">The method.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> Invoke<TDelegate>(this IEmitter<TDelegate> emitter, MethodInfo method) where TDelegate : Delegate
        {
            emitter.Emit(OpCodes.Callvirt, method);
            return emitter;
        }

        /// <summary>
        /// Emits the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="memberExpression">The method expression.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> Invoke<T, TDelegate>(this IEmitter<TDelegate> emitter, Expression<Action<T>> memberExpression) where TDelegate : Delegate
        {
            emitter.Invoke(memberExpression);
            return emitter;
        }

        /// <summary>
        /// Emits the call.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="memberExpression">The method expression.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> Invoke<T, TReturn, TDelegate>(
            this IEmitter<TDelegate> emitter,
            Expression<Func<T, TReturn>> memberExpression) where TDelegate : Delegate
        {
            emitter.Invoke(memberExpression);
            return emitter;
        }

        /// <summary>
        /// Loads the specified i.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the t delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <param name="i">The i.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> Load<TDelegate>(this IEmitter<TDelegate> emitter, int i) where TDelegate : Delegate
        {
            emitter.Emit(OpCodes.Ldarg, i);
            return emitter;
        }
    }
}
