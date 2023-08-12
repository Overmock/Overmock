// ***********************************************************************
// Assembly         : Kimono
// Author           : sbaker
// Created          : 07-30-2023
//
// Last Modified By : sbaker
// Last Modified On : 08-10-2023
// ***********************************************************************
// <copyright file="TypeInterceptor.cs" company="Kimono">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Kimono
{
	/// <summary>
	/// Class TypeInterceptor.
	/// Implements the <see cref="Kimono.Interceptor{TInterface}" />
	/// </summary>
	/// <typeparam name="TInterface">The type of the t interface.</typeparam>
	/// <seealso cref="Kimono.Interceptor{TInterface}" />
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

		/// <summary>
		/// Performs an implicit conversion from <see cref="CallbackInterceptor{TInterface}"/> to <typeparamref name="TInterface"/> cref="TInterface"/>.
		/// </summary>
		/// <param name="interceptor">The interceptor.</param>
		/// <returns>The result of the conversion.</returns>
		public static implicit operator TInterface(CallbackInterceptor<TInterface> interceptor)
		{
			return interceptor.Proxy;
		}
	}
}
