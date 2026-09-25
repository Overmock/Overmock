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
        /// Gets a value indicating whether this interceptor wraps a target instance.
        /// <para>
        /// This is informational only. It must NOT be used to decide anything that is baked into
        /// the generated proxy type, because that type - and its <see cref="MethodMetadata"/> - is
        /// cached per <typeparamref name="T"/> for the life of the process, while having a target
        /// is per-interceptor state.
        /// </para>
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