using System;

namespace Overmocked
{
    /// <summary>
    /// Class Value.
    /// Implements the <see cref="IEquatable{T}" />
    /// Implements the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IEquatable{T}" />
    /// <seealso cref="IFluentInterface" />
    public abstract class Value<T> : IEquatable<T>, IFluentInterface
    {
        /// <inheritdoc />
        public abstract bool Equals(T other);
    }

    /// <summary>
    /// Represents values for mocked methods.
    /// </summary>
    public abstract class Its : Value<Its>, IFluentInterface
    {
        /// <summary>
        /// Anies this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>Any&lt;T&gt;.</returns>
        public static Any<T> Any<T>()
        {
            return new Any<T>();
        }

        /// <summary>
        /// Thises the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns>This&lt;T&gt;.</returns>
        public static This<T> This<T>(T value)
        {
            return new This<T>(value);
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return ((IEquatable<Its>)this).Equals((obj as Its)!);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Interface IAny
    /// Extends the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IFluentInterface" />
    public interface IAny<T> : IFluentInterface
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }

    /// <summary>
    /// Class Any.
    /// Implements the <see cref="Value{T}" />
    /// Implements the <see cref="IAny{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Value{T}" />
    /// <seealso cref="IAny{T}" />
    public class Any<T> : Value<T>, IAny<T>, IEquatable<Any<T>>
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
#pragma warning disable CA1000 // Do not declare static members on generic types
        public static T Value { get; } = new Any<T>();
#pragma warning restore CA1000 // Do not declare static members on generic types

        /// <summary>
        /// Performs an implicit conversion from <see cref="Any{T}"/> to <typeparamref name="T"/>.
        /// </summary>
        /// <param name="any">Any.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator T(Any<T> any) => any.ThisValue!;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T IAny<T>.Value => this;

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool Equals(T other)
        {
            return true;
        }

        /// <inheritdoc />
        public bool Equals(Any<T>? other)
        {
            return true;
        }

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            return base.Equals(obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <inheritdoc/>
        public override string? ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Gets the this value.
        /// </summary>
        /// <value>The this value.</value>
        protected T ThisValue => this;
    }

    /// <summary>
    /// Interface IAm
    /// Extends the <see cref="IAny{T}" />
    /// Extends the <see cref="IFluentInterface" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IAny{T}" />
    /// <seealso cref="IFluentInterface" />
    public interface IAm<T> : IAny<T>, IFluentInterface
    {
    }

    /// <summary>
    /// Class This.
    /// Implements the <see cref="Any{T}" />
    /// Implements the <see cref="IAm{T}" />
    /// Implements the <see cref="IEquatable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Any{T}" />
    /// <seealso cref="IAm{T}" />
    /// <seealso cref="IEquatable{T}" />
    public class This<T> : Any<T>, IAm<T>, IEquatable<T>
    {
        /// <summary>
        /// The value
        /// </summary>
        private readonly T _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="This{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        internal This(T value)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        T IAny<T>.Value => _value!;

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="NotImplementedException"></exception>
        bool IEquatable<T>.Equals(T other)
        {
            return other != null && other.Equals(this);
        }
    }
}
