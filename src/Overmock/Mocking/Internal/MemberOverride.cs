
using Overmock.Runtime;

namespace Overmock.Mocking.Internal
{
	/// <summary>
	/// 
	/// </summary>
	public class MemberOverride : ThrowExceptionOverride, IOverride
	{
		internal MemberOverride(
			Func<object>? returnProvider = default,
			Exception? exception = default)
			: base(exception)
		{
			ReturnProvider = returnProvider;
		}

		/// <summary>
		/// 
		/// </summary>
		public Func<object>? ReturnProvider { get; }
	}
}