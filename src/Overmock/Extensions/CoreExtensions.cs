
using System;
using System.Globalization;

namespace Overmock
{
    /// <summary>
    /// Class OvermockExtensions.
    /// </summary>
    public static partial class OvermockExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="overmock"></param>
        /// <returns></returns>
        public static IOvermock<TReturn> AsMock<TReturn>(this IOvermock overmock) where TReturn : class
        {
            return new Overmock<TReturn>((TReturn)overmock.GetTarget());
        }

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

        /// <summary>
        /// Generic overload for <see cref="Type.IsAssignableFrom(Type)"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool Implements<T>(this Type type)
        {
            return typeof(T).IsAssignableFrom(type);
        }
    }
}
