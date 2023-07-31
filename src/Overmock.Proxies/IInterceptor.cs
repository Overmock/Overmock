﻿
namespace Overmock.Proxies
{
	public interface IInterceptor
	{
		/// <summary>
		/// 
		/// </summary>
		string TypeName { get; }

		Type TargetType { get; }

		void MemberInvoked(InvocationContext context);
	}

	public interface IInterceptor<T> : IInterceptor where T : class
	{
		T Target { get; }
	}
}