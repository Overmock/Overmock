﻿
namespace Overmock.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxy<T> : IProxy where T : class
	{
		//void RegisterCallback(Func<RuntimeContext, object[], object> memberInvoked);
	}

	/// <summary>
	/// 
	/// </summary>
	public interface IProxy
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void InitializeProxyContext(ProxyContext context);

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		Type GetTargetType();
		
		///// <summary>
		///// 
		///// </summary>
		///// <param name="parameters"></param>
		///// <returns></returns>
		//object? MemberInvoked(RuntimeContext context, object[] parameters);
	}
}