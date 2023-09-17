namespace Kimono.Interceptors
{
    /// <summary>
    /// An <see cref="Interceptor{T}" /> targeting an object.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Interceptor{T}" />
    public class TargetedCallbackInterceptor<T> : CallbackInterceptor<T> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TargetedCallbackInterceptor{T}"/> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="memberInvoked"></param>
        public TargetedCallbackInterceptor(T target, InvocationAction memberInvoked) : base(memberInvoked)
        {
            Target = target;
        }
    }
}
