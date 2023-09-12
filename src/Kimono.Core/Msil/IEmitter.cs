using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Core.Msil
{
    /// <summary>
    /// Interface IEmitter
    /// </summary>
    public interface IEmitter
    {
        /// <summary>
        /// Gets the backing MSIL generator.
        /// </summary>
        /// <value>The il generator.</value>
        ILGenerator IlGenerator { get; }

        /// <summary>
        /// Emits the specified op code.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <param name="constructor">The constructor.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Emit(OpCode opCode, ConstructorInfo constructor);

        /// <summary>
        /// Emits the specified op code.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <param name="method">The method.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Emit(OpCode opCode, MethodInfo method);

        /// <summary>
        /// Emits the specified op code.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <param name="type">The type.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Emit(OpCode opCode, Type type);

        /// <summary>
        /// Emits the specified op code.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <param name="i">The i.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Emit(OpCode opCode, int i);

        /// <summary>
        /// Emits the specified op code.
        /// </summary>
        /// <param name="opCode">The op code.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Emit(OpCode opCode);

        /// <summary>
        /// Emits the call.
        /// </summary>
        /// <param name="opcode">The opcode.</param>
        /// <param name="method">The method.</param>
        IEmitter EmitCall(OpCode opcode, MethodInfo method);

        /// <summary>
        /// Nops the specified emitter.
        /// </summary>
        /// <returns>IEmitter.</returns>
        IEmitter Nop();

        /// <summary>
        /// Nops this instance.
        /// </summary>
        /// <returns>IEmitter.</returns>
        IEmitter Ret();

        /// <summary>
        /// Pops this instance.
        /// </summary>
        /// <returns>IEmitter.</returns>
        IEmitter Pop();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEmitter Box(Type type);

        /// <summary>
        /// Loads <see cref="OpCodes.Ldarg"/> the specified indexes.
        /// </summary>
        /// <param name="indexes"></param>
        /// <returns>IEmitter.</returns>
        IEmitter Load(params int[] indexes);

        /// <summary>
        /// Loads the specified index.
        /// </summary>
        /// <param name="i">The index.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Load(int i);

        /// <summary>
        /// Bases the ctor.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>IEmitter.</returns>
        IEmitter BaseCtor(ConstructorInfo constructor);

        /// <summary>
        /// Creates new ctor.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <returns>IEmitter.</returns>
        IEmitter NewCtor(ConstructorInfo constructor);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="method">The method.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Invoke(MethodInfo method);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Invoke<T>(Expression<Action<T>> memberExpression);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>IEmitter.</returns>
        IEmitter Invoke<T, TReturn>(Expression<Func<T, TReturn>> memberExpression);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="propertyInfo">The property information.</param>
        /// <returns>IEmitter.</returns>
        ICallPropertyGetOrSet InvokeProperty(PropertyInfo propertyInfo);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>IEmitter.</returns>
        ICallPropertySet InvokeProperty<T>(Expression<Action<T>> memberExpression);

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TReturn">The type of the t return.</typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <returns>IEmitter.</returns>
        ICallPropertyGet InvokeProperty<T, TReturn>(Expression<Func<T, TReturn>> memberExpression);

        /// <summary>
        /// Casts the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>IEmitter.</returns>
        IEmitter CastTo(Type type);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Label Label();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="label"></param>
        IEmitter Mark(Label label);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <param name="label"></param>
        IEmitter Emit(OpCode code, Label label);

        /// <summary>
        /// Nops this instance.
        /// </summary>
        /// <returns>IEmitter.</returns>
        LocalBuilder DeclareLocal(Type type);
    }

    /// <summary>
    /// Interface IEmitter
    /// </summary>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    public interface IEmitter<TDelegate> : IEmitter where TDelegate : Delegate
    {
    }
}
