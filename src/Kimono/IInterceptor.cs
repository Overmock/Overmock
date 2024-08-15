using System;
using System.ComponentModel;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInterceptor : IFluentInterface
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="genericParameters"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        object? HandleInvocation(int methodId, Type[] genericParameters, object[] parameters);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IInterceptor<out T> : IInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool ContainsTarget { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDisposableInterceptor<T> : IInterceptor<T>, IDisposable
    {
    }
}