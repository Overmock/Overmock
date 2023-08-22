using System;

namespace Kimono
{
	/// <summary>
	/// Class KimonoExtensions.
	/// </summary>
	public static class KimonoExtensions
    {
		/// <summary>
		/// Determines whether the specified target is interface.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if the specified target is interface; otherwise, <c>false</c>.</returns>
		internal static bool IsInterface(this IInterceptor target)
        {
            return target.TargetType.IsInterface();
        }

		/// <summary>
		/// Determines whether the specified target is delegate.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if the specified target is delegate; otherwise, <c>false</c>.</returns>
		internal static bool IsDelegate(this IInterceptor target)
        {
            return target.TargetType.IsDelegate();
        }

		/// <summary>
		/// Determines whether the specified target is interface.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if the specified target is interface; otherwise, <c>false</c>.</returns>
		internal static bool IsInterface(this Type target)
        {
            return target is { IsInterface: true };
        }

		/// <summary>
		/// Determines whether the specified target is delegate.
		/// </summary>
		/// <param name="target">The target.</param>
		/// <returns><c>true</c> if the specified target is delegate; otherwise, <c>false</c>.</returns>
		internal static bool IsDelegate(this Type target)
        {
            return Constants.DelegateType.IsAssignableFrom(target);
		}

		/// <summary>
		/// Applies the format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The arguments.</param>
		/// <returns>System.String.</returns>
		internal static string ApplyFormat(this string format, params object[] args)
		{
			return Constants.Format(() => format, args);
		}
	}
}