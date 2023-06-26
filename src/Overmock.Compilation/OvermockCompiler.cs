using Overmock.Compilation.IL;
using Overmock.Compilation.Roslyn;

namespace Overmock.Compilation
{
	public class OvermockCompiler
	{
		public static IOvermockBuilder IntermediateLanguageBuilder()
		{
			return new ILOvermockBuilder();
		}

		public static IOvermockBuilder RoslynBuilder()
		{
			return new RoslynOvermockBuilder();
		}

		public static IOvermockBuilder CodeDomBuilder()
		{
			return new RoslynOvermockBuilder();
		}
	}
}
