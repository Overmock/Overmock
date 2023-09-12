using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Core.Msil
{
    /// <summary>
    /// Class Emitter. This class cannot be inherited.
    /// </summary>
    public abstract partial class Emitter : IEmitter
    {
        /// <inheritdoc />
        IEmitter IEmitter.Emit(OpCode opCode, ConstructorInfo constructor)
        {
            _emitter.Emit(opCode, constructor);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.Emit(OpCode opCode, MethodInfo method)
        {
            _emitter.Emit(opCode, method);
            return this;
        }

        IEmitter IEmitter.Emit(OpCode opCode, Type type)
        {
            _emitter.Emit(opCode, type);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.Emit(OpCode opCode, int i)
        {
            _emitter.Emit(opCode, i);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.Emit(OpCode opCode)
        {
            _emitter.Emit(opCode);
            return this;
        }

        IEmitter IEmitter.Emit(OpCode code, Label label)
        {
            _emitter.Emit(code, label);
            return this;
        }

        IEmitter IEmitter.EmitCall(OpCode opcode, MethodInfo method)
        {
            _emitter.EmitCall(opcode, method, null);
            return this;
        }
    }
}
