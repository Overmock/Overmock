using System.Diagnostics;

namespace Kimono.Core
{
    [DebuggerDisplay(null, Name = "Current")]
    public class MethodId
    {
        private int _current;

        private MethodId()
        {
            _current = 0;
        }

        public static MethodId Create() { return new MethodId(); }

        public static MethodId operator ++(MethodId methodId)
        {
            methodId._current++;
            return methodId;
        }

        public static implicit operator int(MethodId methodId)
        {
            return methodId._current;
        }

        public int Current => _current;

        public MethodId Next()
        {
            ++_current;
            return this;
        }

        public int NextInt()
        {
            return ++_current;
        }

        public override string ToString()
        {
            return Current.ToString();
        }
    }
}
