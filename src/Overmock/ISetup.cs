using System;

namespace Overmock
{
	/// <summary>
	/// Represents a member that can setup an Exception to throw when calling the overmock.
	/// </summary>
	public interface ISetup : IFluentInterface
	{
		/// <summary>
		/// Converts to becalled.
		/// </summary>
		void ToBeCalled();

		/// <summary>
		/// Converts to becalled.
		/// </summary>
		/// <param name="times">The times.</param>
		void ToBeCalled(Times times);

		/// <summary>
		/// Specifies the exception to throw when the overmocked member is called.
		/// </summary>
		/// <param name="exception">The exception to throw.</param>
		void ToThrow(Exception exception);
    }

	/// <summary>
	/// Interface ISetup
	/// Extends the <see cref="Overmock.ISetup" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Overmock.ISetup" />
	public interface ISetup<in T> : ISetup where T : class
	{
		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="action">The action.</param>
		void ToCall(Action<OvermockContext> action);

		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="times">The times.</param>
		void ToCall(Action<OvermockContext> action, Times times);
	}

	/// <summary>
	/// Represents a member that can setup a return value.
	/// </summary>
	/// <typeparam name="TReturn">The type of the t return.</typeparam>
	public interface ISetupReturn<in TReturn> : ISetup
    {
		/// <summary>
		/// Sets the value used as the result when calling this overmock's object.
		/// </summary>
		/// <param name="resultProvider">The result provider.</param>
		void ToReturn(Func<TReturn> resultProvider);

		/// <summary>
		/// Sets the value used as the result when calling this overmock's object.
		/// </summary>
		/// <param name="result">The result.</param>
		void ToReturn(TReturn result);
    }

	/// <summary>
	/// Interface ISetup
	/// Extends the <see cref="Overmock.ISetup{T}" />
	/// Extends the <see cref="Overmock.ISetupReturn{TReturn}" />
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TReturn">The type of the t return.</typeparam>
	/// <seealso cref="Overmock.ISetup{T}" />
	/// <seealso cref="Overmock.ISetupReturn{TReturn}" />
	public interface ISetup<in T, in TReturn> : ISetup<T>, ISetupReturn<TReturn> where T : class
	{
		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="callback">The callback.</param>
		void ToCall(Func<OvermockContext, TReturn> callback);

		/// <summary>
		/// Converts to call.
		/// </summary>
		/// <param name="callback">The callback.</param>
		/// <param name="times">The times.</param>
		void ToCall(Func<OvermockContext, TReturn> callback, Times times);
    }
}
