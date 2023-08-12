using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kimono
{
	/// <summary>
	/// Represents a member invocation.
	/// </summary>
	public interface IInvocationContext
	{
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
		/// Gets the parent context.
		/// </summary>
		/// <value>The parent context.</value>
		RuntimeContext ParentContext { get; }
		
		/// <summary>
		/// Gets or sets the return value.
		/// </summary>
		/// <value>The return value.</value>
		object? ReturnValue { get; set; }

		/// <summary>
		/// Invokes the target.
		/// </summary>
		/// <param name="setReturnValue">if set to <c>true</c> [set return value].</param>
		void InvokeTarget(bool setReturnValue = true);
	}
}
