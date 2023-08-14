using System.Reflection;

namespace Kimono
{
	/// <summary>
	/// Represents a member invocation.
	/// </summary>
	public interface IInvocationContext : IFluentInterface
	{
		/// <summary>
		/// Gets a value indicating whether <see cref="Invoke(bool,bool)"/> has been called previously.
		/// </summary>
		/// <value><c>true</c> if [target invoked]; otherwise, <c>false</c>.</value>
		public bool TargetInvoked { get; }

		/// <summary>
		/// Gets the interceptor.
		/// </summary>
		/// <value>The interceptor.</value>
		IInterceptor Interceptor { get; }

		/// <summary>
		/// Gets the member.
		/// </summary>
		/// <value>The member.</value>
		MemberInfo Member { get; }
		
		/// <summary>
		/// Gets the name of the member.
		/// </summary>
		/// <value>The name of the member.</value>
		string MemberName { get; }
		
		/// <summary>
		/// Gets the method.
		/// </summary>
		/// <value>The method.</value>
		MethodInfo Method { get; }
		
		/// <summary>
		/// Gets the parameters.
		/// </summary>
		/// <value>The parameters.</value>
		Parameters Parameters { get; }
		
		/// <summary>
		/// Gets or sets the return value.
		/// </summary>
		/// <value>The return value.</value>
		object? ReturnValue { get; set; }

		/// <summary>
		/// Gets the specified name.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">The name.</param>
		/// <returns>T.</returns>
		T GetParameter<T>(string name);

		/// <summary>
		/// Invokes the target.
		/// </summary>
		/// <param name="setReturnValue">if set to <c>true</c> [set return value].</param>
		/// <param name="force">if set to <c>true</c> forces the call to be invoked regardless if it's already been called successfully.</param>
		void Invoke(bool setReturnValue = true, bool force = false);
	}
}
