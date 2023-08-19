using System.Reflection;
using System.Reflection.Emit;

namespace Kimono.Emit
{
    /// <summary>
    /// Class MsilEmitter.
    /// Implements the <see cref="IEmitter" />
    /// </summary>
    /// <seealso cref="IEmitter" />
    public sealed class MsilEmitter : Emitter, IEmitter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsilEmitter"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        public MsilEmitter(ILGenerator emitter) : base(emitter)
        {
        }
    }

    /// <summary>
    /// Class MsilEmitter.
    /// Implements the <see cref="IEmitter{TDelegate}" />
    /// </summary>
    /// <seealso cref="IEmitter{TDelegate}" />
    public sealed class MsilEmitter<TDelegate> : Emitter, IEmitter<TDelegate> where TDelegate : Delegate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MsilEmitter{TDelegate}"/> class.
        /// </summary>
        /// <param name="emitter">The emitter.</param>
        public MsilEmitter(ILGenerator emitter) : base(emitter)
        {
        }
    }
}
