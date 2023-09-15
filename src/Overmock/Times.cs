using Kimono;
using System.Diagnostics;
using System.Globalization;

namespace Overmocked
{
    /// <summary>
    /// Struct Times
    /// </summary>
    [DebuggerDisplay(null, Name = "Value")]
    public readonly struct Times
    {
        /// <summary>
        /// The times
        /// </summary>
        private readonly int _times;

        /// <summary>
        /// Initializes a new instance of the <see cref="Times"/> struct.
        /// </summary>
        /// <param name="times">The times.</param>
        public Times(int times = -1)
        {
            _times = times;
        }

        /// <summary>
        /// The once
        /// </summary>
        public readonly static Times Once = new Times(1);

        /// <summary>
        /// The zero
        /// </summary>
        public readonly static Times Zero = new Times(0);

        /// <summary>
        /// Any
        /// </summary>
        public readonly static Times Any = new Times(-1);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public int Value => _times;

        /// <summary>
        /// Performs an implicit conversion from <see cref="Times"/> to <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="times">The times.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator int(Times times)
        {
            return times._times;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.Int32"/> to <see cref="Times"/>.
        /// </summary>
        /// <param name="times">The times.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Times(int times)
        {
            return times == 1
                ? Once : times == 0
                    ? Zero : times < 0
                        ? Any : new Times(times);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _times.ToString(CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Throws if invalid.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <exception cref="KimonoException">Method called: Expected: {_times}, Actual: {count}</exception>
        internal void ThrowIfInvalid(int count)
        {
            // -1 represents inifinity.
            if (_times == -1)
            {
                return;
            }

            if (count > _times)
            {
                throw new KimonoException($"Method called: Expected: {_times}, Actual: {count}");
            }
        }
    }
}
