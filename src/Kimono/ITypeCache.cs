using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITypeCache<T> : IFluentInterface where T : class
    {
        /// <summary>
        /// Determines whether this instance contains the object.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns><c>true</c> if [contains] [the specified type]; otherwise, <c>false</c>.</returns>
        bool Contains(Type type);

        /// <summary>
        /// Tries the get.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the key is present, <c>false</c> otherwise.</returns>
        bool TryGet(Type type, out T? value);

        /// <summary>
        /// Tries the set.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool TrySet(Type type, T value);

        /// <summary>
        /// Gets the value or default if not found.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        T? GetValueOrDefault(Type type);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypeCache<T> : ITypeCache<T> where T : class
    {
        private readonly ConcurrentDictionary<Type, T> _cache = new ConcurrentDictionary<Type, T>();

        /// <inheritdoc />
        public virtual bool Contains(Type type) => _cache.ContainsKey(type);

        /// <inheritdoc />
        public virtual bool TryGet(Type type, out T? value) => _cache.TryGetValue(type, out value);

        /// <inheritdoc />
        public virtual bool TrySet(Type type, T value) => _cache.TryAdd(type, value);

        /// <inheritdoc />
        public T? GetValueOrDefault(Type type) => _cache.GetValueOrDefault(type);
    }
}