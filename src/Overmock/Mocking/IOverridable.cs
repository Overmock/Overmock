using System.Reflection;

namespace Overmock.Mocking
{
	/// <summary>
	/// 
	/// </summary>
	public interface IOverridable
	{
		/// <summary>
		/// Gets the overrides for this overmock.
		/// </summary>
		/// <returns></returns>
		IEnumerable<IOverride> GetOverrides();

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		MemberInfo GetTarget();
	}
}