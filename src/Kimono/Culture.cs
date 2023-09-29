using System;
using System.Globalization;

namespace Kimono
{
    internal static class Culture
    {
        public static readonly IFormatProvider Current = CultureInfo.CurrentCulture;
    }
}
