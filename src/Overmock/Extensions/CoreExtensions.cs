
using System.Globalization;

namespace Overmock
{
	/// <summary>
	/// Class OvermockExtensions.
	/// </summary>
	public static partial class OvermockExtensions
    {
		/// <summary>
		/// Applies the format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>System.String.</returns>
		internal static string ApplyFormat(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }
    }
}
