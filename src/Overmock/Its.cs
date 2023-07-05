namespace Overmock
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class Value<T> : IEquatable<T>, IFluentInterface
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public abstract bool Equals(T? other);
	}

	/// <summary>
	/// Represents values for mocked methods.
	/// </summary>
	public abstract class Its : Value<Its>, IFluentInterface
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public static Any<T> Any<T>()
		{
			return new Any<T>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="value"></param>
		/// <returns></returns>
		public static This<T> This<T>(T value)
		{
			return new This<T>(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object? obj)
		{
			return ((IEquatable<Its>)this).Equals(obj as Its);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAny<T> : IFluentInterface
	{
		/// <summary>
		/// 
		/// </summary>
		T Value { get; }
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Any<T> : Value<T>, IAny<T>, IEquatable<Any<T>>
	{
		/// <summary>
		/// 
		/// </summary>
#pragma warning disable CA1000 // Do not declare static members on generic types
		public static T Value { get; } = new Any<T>();
#pragma warning restore CA1000 // Do not declare static members on generic types

		/// <summary>
		/// 
		/// </summary>
		/// <param name="any"></param>
		public static implicit operator T(Any<T> any) => any.ThisValue!;

		T IAny<T>.Value => this;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Its? other)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public override bool Equals(T? other)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(Any<T>? other)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		protected T ThisValue => this;
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAm<T> : IAny<T>, IFluentInterface
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class This<T> : Any<T>, IAm<T>, IEquatable<T>
	{
		private readonly T? _value;

		internal This(T? value)
		{
			_value = value;
		}

		T IAny<T>.Value => this;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		bool IEquatable<T>.Equals(T? other)
		{
			return other != null && other.Equals(this);
		}
	}
}
