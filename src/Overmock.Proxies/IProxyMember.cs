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

		Func<object, object[]?, object?> CreateDelegate();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        MemberInfo GetMember();
    }
}