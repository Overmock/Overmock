using System.Diagnostics;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    [DebuggerDisplay(null, Name = "Current")]
    public class MethodId
    {
        private int _current;

        private MethodId()
        {
            _current = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MethodId Create() => new MethodId();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <returns></returns>
        public static MethodId operator ++(MethodId methodId)
        {
            methodId._current++;
            return methodId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        public static implicit operator int(MethodId methodId)
        {
            return methodId._current;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Current => _current;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MethodId Next()
        {
            ++_current;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int NextInt()
        {
            return ++_current;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Current.ToString(Culture.Current);
        }
    }
}
