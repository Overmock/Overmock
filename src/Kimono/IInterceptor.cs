
namespace Kimono
{
	public interface IInterceptor
	{
		/// <summary>
		/// 
		/// </summary>
		string TypeName { get; }

		Type TargetType { get; }

		object GetTarget();

		void MemberInvoked(InvocationContext context);
	}

	public interface IInterceptor<T> : IInterceptor where T : class
	{
		T? Target { get; }
	}
}
