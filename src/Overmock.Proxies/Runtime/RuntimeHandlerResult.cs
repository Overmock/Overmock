namespace Overmock.Proxies
{
	/// <summary>
	/// 
	/// </summary>
	public class RuntimeHandlerResult
	{
		/// <summary>
		/// 
		/// </summary>
        public static RuntimeHandlerResult Empty = new RuntimeHandlerResult(new object());

        internal RuntimeHandlerResult(object? result)
		{
			Result = result;
		}

		/// <summary>
		/// 
		/// </summary>
		public object? Result { get; }
	}
}
