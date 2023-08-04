using System.Reflection;

namespace Overmock.Proxies
{
    public interface IProxyMember
    {
        /// <summary>
        /// 
        /// </summary>
        MethodInfo Method { get; }

        /// <summary>
        /// 
        /// </summary>
		string Name { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		object? GetDefaultReturnValue();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        MemberInfo GetMember();
    }
}