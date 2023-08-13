﻿
namespace Kimono
{
	/// <summary>
	/// An <see cref="Kimono.Interceptor{T}" /> targeting an object.
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
		public TargetedCallbackInterceptor(T target, Action<IInvocationContext> memberInvoked) : base(memberInvoked)
		{
			Target = target;
		}
	}
}