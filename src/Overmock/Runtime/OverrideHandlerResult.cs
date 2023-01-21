namespace Overmock.Runtime
{
	/// <summary>
	/// 
	/// </summary>
	public class OverrideHandlerResult
	{
		internal OverrideHandlerResult(object result)
		{
			Result = result;
		}

		/// <summary>
		/// 
		/// </summary>
		public object Result { get; }
	}
}
