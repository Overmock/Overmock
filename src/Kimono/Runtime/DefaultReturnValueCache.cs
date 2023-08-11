
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace Kimono.Runtime
{
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
