using System.Reflection;

namespace Overmock.Runtime.Proxies
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IProxy<T> : IProxy where T : class
    {
    }

	/// <summary>
	/// 
	/// </summary>
	public interface IProxy
	{
		/// <summary>
		/// 
		/// </summary>
		IOvermock Target { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void InitializeOvermockContext(OvermockRuntimeContext context);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="method"></param>
		/// <param name="parameters"></param>
		/// <returns></returns>
		object HandleMethodCall(MethodInfo method, params object[] parameters);
	}
}