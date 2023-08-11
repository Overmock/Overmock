using System.Reflection;

namespace Kimono
{
    public interface IProxyMember
	{
		/// <summary>
		/// 
		/// </summary>
		MemberInfo Member { get; }

		/// <summary>
		/// 
		/// </summary>
		MethodInfo Method { get; }

        /// <summary>
        /// 
        /// </summary>
		string Name { get; }

		Func<object, object[]?, object?> CreateDelegate();
    }
}