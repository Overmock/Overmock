using Kimono;

namespace Kimono.Runtime
{
	/// <summary>
	/// 
	/// </summary>
	public class EmptyOverridesRuntimeHandler : RuntimeHandlerBase
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="runtimeContext"></param>
		public EmptyOverridesRuntimeHandler(RuntimeContext runtimeContext) : base(runtimeContext)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parameters"></param>
		/// <returns></returns>
		protected override RuntimeHandlerResult HandleCore(IProxy proxy, params object[] parameters)
		{
			return RuntimeHandlerResult.Empty;
		}
	}
}