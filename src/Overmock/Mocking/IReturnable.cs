
using System;

namespace Overmock.Mocking
{
	/// <summary>
	/// Interface IReturnable
	/// Extends the <see cref="Overmock.Mocking.ICallable" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.Mocking.ICallable" />
	public interface IReturnable<T> : ICallable
	{
		/// <summary>
		/// Returnses the specified value.
		/// </summary>
		/// <param name="value">The value.</param>
		void Returns(T value);

		/// <summary>
		/// Returnses the specified function.
		/// </summary>
		/// <param name="func">The function.</param>
		void Returns(Func<T> func);

		/// <summary>
		/// Callses the specified function.
		/// </summary>
		/// <param name="func">The function.</param>
		/// <param name="times">The times.</param>
		void Calls(Func<OvermockContext, T> func, Times times);
	}
}