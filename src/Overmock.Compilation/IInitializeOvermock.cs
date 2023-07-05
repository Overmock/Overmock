using Overmock.Runtime;

namespace Overmock.Compilation
{
	/// <summary>
	/// 
	/// </summary>
	public interface IInitializeOvermock
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		void InitializeOvermock(OvermockRuntimeContext context);
	}
}
