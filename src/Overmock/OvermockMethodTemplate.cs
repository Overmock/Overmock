using Overmock.Runtime;
using System.Reflection;

namespace Overmock
{
	/// <summary>
	/// Do not use. Used for testing.
	/// </summary>
	public class OvermockMethodTemplate
	{
#pragma warning disable IDE1006 // Naming Styles
		private OvermockContext? ___context___;
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void InitializeOvermockContext(OvermockContext context)
		{
			___context___ = context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="OvermockException"></exception>
		public object TestMethod(string name)
		{
			var handle = ___context___.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
			var result = handle.Handle(name);
			return result.Result;
		}
	}
}