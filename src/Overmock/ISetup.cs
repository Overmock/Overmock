using Overmock.Runtime;

namespace Overmock
{
    /// <summary>
    /// Represents a member that can setup an Exception to throw when calling the overmock.
    /// </summary>
    public interface ISetup : IFluentInterface
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
    public interface ISetup<in T> : ISetup where T : class
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		void ToCall(Action<RuntimeContext> action);
	}

    /// <summary>
    /// Represents a member that can setup a return value.
    /// </summary>
    /// <typeparam name="TReturn"></typeparam>
    public interface ISetupReturn<in TReturn> : ISetup
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
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public interface ISetup<in T, in TReturn> : ISetup<T>, ISetupReturn<TReturn> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        void ToCall(Func<RuntimeContext, TReturn> callback);
    }
}
