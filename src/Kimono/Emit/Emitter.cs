using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Kimono.Emit
{
    public abstract partial class Emitter : IEmitter
    {
        private readonly ILGenerator _emitter;

        /// <summary>
        /// Initializes a new instance of the <see cref="Emitter"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        protected Emitter(ILGenerator emitter)
        {
            _emitter = emitter;
        }

        /// <summary>
        /// Gets the il generator.
        /// </summary>
        /// <value>The il generator.</value>
        public ILGenerator IlGenerator => _emitter;

        /// <summary>
        /// Creates an <see cref="IEmitter{TDelegate}"/> using the specified emitter.
        /// </summary>
        /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
        /// <param name="emitter">The emitter.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter<TDelegate> For<TDelegate>(ILGenerator emitter) where TDelegate : Delegate
        {
            return new MsilEmitter<TDelegate>(emitter);
        }

        /// <summary>
        /// Creates an <see cref="IEmitter{TDelegate}" /> using the specified emitter.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        /// <returns>IEmitter&lt;TDelegate&gt;.</returns>
        public static IEmitter For(ILGenerator emitter)
        {
            return new MsilEmitter(emitter);
        }

        IEmitter IEmitter.Pop()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        IEmitter IEmitter.Nop()
        {
            _emitter.Emit(OpCodes.Nop);
            return this;
        }

        /// <inheritdoc />
        IEmitter IEmitter.Ret()
        {
            _emitter.Emit(OpCodes.Ret);
            return this;
        }

        IEmitter IEmitter.Load(int i)
        {
            _emitter.Emit(OpCodes.Ldarg, i);
            return this;
        }

        IEmitter IEmitter.Load(params int[] indexes)
        {
            ref var reference = ref MemoryMarshal.GetReference(indexes.AsSpan());

            for (int i = 0; i < indexes.Length; i++)
            {
                ref var index = ref Unsafe.Add(ref reference, i);
                _emitter.Emit(OpCodes.Ldarg, index);
            }

            return this;
        }

        IEmitter IEmitter.CastTo(Type type)
        {
            _emitter.Emit(OpCodes.Castclass, type);
            return this;
        }
    }
}
