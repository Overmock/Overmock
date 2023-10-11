using System;

namespace Overmocked.Matchable
{
    /// <summary>
    /// Class Value.
    /// Implements the <see cref="IEquatable{T}" />
    /// Implements the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IEquatable{T}" />
    /// <seealso cref="IFluentInterface" />
    public abstract class ParameterValue<T> : IEquatable<T>, IFluentInterface
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual T Value => default!;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="param"></param>
        public static implicit operator T(ParameterValue<T> param)
        {
            return param.Value;
        }

        /// <inheritdoc />
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
        public abstract bool Equals(T other);
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    }
}
