using Overmock.Runtime.Proxies;

namespace Overmock.Compilation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IInitializeOvermock
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void InitializeOvermock(ProxyOverrideContext context);
	}
}
