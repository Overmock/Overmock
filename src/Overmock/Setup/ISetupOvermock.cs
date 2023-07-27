using Overmock.Runtime;

namespace Overmock.Setup
{
	/// <summary>
	/// Represents a member that can setup a return value.
	/// </summary>
	/// <typeparam name="TReturn"></typeparam>
	public interface ISetupReturn<in TReturn>
	{
		/// <summary>
		/// Sets the value used as the result when calling this overmock's object.
		/// </summary>
		/// <param name="resultProvider"></param>
		void ToReturn(Func<TReturn> resultProvider);

		/// <summary>
		/// Sets the value used as the result when calling this overmock's object.
		/// </summary>
		/// <param name="result"></param>
		void ToReturn(TReturn result);
	}

	/// <summary>
	/// Represents a member that can setup an Exception to throw when calling the overmock.
	/// </summary>
	public interface ISetupOvermock
	{
		/// <summary>
		/// Specifies the exception to throw when the overmocked member is called.
		/// </summary>
		/// <param name="exception">The exception to throw.</param>
		void ToThrow(Exception exception);
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ISetupOvermock<in T> : ISetupOvermock where T : class
	{
	}

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="TReturn"></typeparam>
	public interface ISetupOvermock<in T, in TReturn> : ISetupOvermock<T>, ISetupReturn<TReturn> where T : class
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="callback"></param>
		/// <returns></returns>
		void ToCall(Func<RuntimeContext, TReturn> callback);
	}
}
