using Overmock.Runtime;

namespace Overmock
{
    public static partial class OvermockExtensions
    {
        internal static string ApplyFormat(this string format, params object[] args)
        {
            return Constants.Format(() => format, args);
        }
    }
}
