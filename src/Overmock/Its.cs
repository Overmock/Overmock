using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Overmock
{
	/// <summary>
	/// Represents values for mocked methods.
	/// </summary>
	public abstract class Its : IEquatable<Its>, IFluentInterface
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
		public static Its This<T>(T value)
		{
			return new This<T>(value);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public abstract bool Equals(Its? other);

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
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class Any<T> : Its, IAny<T>, IEquatable<T>
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="any"></param>
		public static implicit operator T(Any<T> any) => default!;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public bool Equals(T? other)
		{
			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(Its? other)
		{
			return true;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IAm<T> : IFluentInterface
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class This<T> : Its, IAm<T>, IEquatable<T>
	{
		private readonly T? _value;

		internal This(T? value)
		{
			_value = value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		public override bool Equals(Its? other)
		{
			return _value?.Equals(other) ?? other == null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="other"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		bool IEquatable<T>.Equals(T? other)
		{
			throw new NotImplementedException();
		}
	}
}
