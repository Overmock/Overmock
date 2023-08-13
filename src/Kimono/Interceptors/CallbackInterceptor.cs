namespace Kimono.Interceptors
{
    /// <summary>
    /// Class TypeInterceptor.
    /// Implements the <see cref="Interceptor{TInterface}" />
    /// </summary>
    /// <typeparam name="TInterface">The type of the t interface.</typeparam>
    /// <seealso cref="Interceptor{TInterface}" />
    public class CallbackInterceptor<TInterface> : Interceptor<TInterface> where TInterface : class
    {
        /// <summary>
        /// The member invoked
        /// </summary>
        private readonly Action<IInvocationContext> _memberInvoked;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallbackInterceptor{TInterface}"/> class.
        /// </summary>
        /// <param name="memberInvoked">The member invoked.</param>
        public CallbackInterceptor(Action<IInvocationContext> memberInvoked) : base(default)
        {
            _memberInvoked = memberInvoked;
        }

        /// <summary>
        /// Members the invoked.
        /// </summary>
        /// <param name="context">The context.</param>
        protected override void MemberInvoked(IInvocationContext context)
        {
            _memberInvoked.Invoke(context);
        }
    }
}
