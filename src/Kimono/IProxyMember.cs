using System.Reflection;

namespace Kimono
{
	/// <summary>
	/// Interface IProxyMember
	/// </summary>
	public interface IProxyMember : IFluentInterface
	{
		/// <summary>
		/// Gets the member.
		/// </summary>
		/// <value>The member.</value>
		MemberInfo Member { get; }

		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		MethodInfo Method { get; }

		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; }

		/// <summary>
		/// Creates the delegate.
		/// </summary>
		/// <returns>Func&lt;System.Object, System.Nullable&lt;System.Object&gt;[], System.Nullable&lt;System.Object&gt;&gt;.</returns>
		Func<object, object[]?, object?> CreateDelegate();
    }
}