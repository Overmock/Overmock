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
		private OvermockContext? ___context;
#pragma warning restore IDE1006 // Naming Styles

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public void InitializeOvermockContext(OvermockContext context)
		{
			___context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		/// <exception cref="OvermockException"></exception>
		[Overmock("71a8440c-ba80-472c-bc31-a3736c3e5b4c")]
		public object TestMethod(string name)
		{
			var handle = ___context.Get((MethodInfo)MethodBase.GetCurrentMethod()!);
			var result = handle.Handle(name);
			return (Type)result.Result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="handler"></param>
		/// <returns></returns>
		/// <exception cref="OvermockException"></exception>
		[Overmock("17a8440c-ba80-472c-bc31-a3736c3e5b4c")]
		public object TestMethod2(IOverrideHandler handler)
		{
			var result = handler.Handle();
			return (Type)result.Result;
		}
	}
}