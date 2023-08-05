
using System.Globalization;

namespace Overmock
{
    public static partial class OvermockExtensions
    {
        internal static string ApplyFormat(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
