using System.Collections.Concurrent;

namespace Kimono.Runtime
{
	/// <summary>
	/// Class DefaultReturnValueCache.
	/// </summary>
	internal static class DefaultReturnValueCache
	{
		private static readonly ConcurrentDictionary<Type, object?> _typeCache = new()
		{
			[typeof(sbyte)] = default(sbyte),
			[typeof(byte)] = default(byte),
			[typeof(short)] = default(short),
			[typeof(ushort)] = default(ushort),
			[typeof(int)] = default(int),
			[typeof(uint)] = default(uint),
			[typeof(long)] = default(long),
			[typeof(ulong)] = default(ulong),
			[typeof(char)] = default(char),
			[typeof(bool)] = default(bool),
			[typeof(float)] = default(float),
			[typeof(double)] = default(double),
			[typeof(ValueType)] = default(ValueType),
			[typeof(Enum)] = default(Enum),
			[typeof(object)] = default
		};

		/// <summary>
		/// Gets the default value.
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns>System.Nullable&lt;System.Object&gt;.</returns>
		public static object? GetDefaultValue(Type type)
		{
			if (_typeCache.TryGetValue(type, out var value))
			{
				return value;
			}

			return null;
		}
	}
}
