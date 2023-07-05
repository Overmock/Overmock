using Overmock.Compilation.IL;
using Overmock.Compilation.Roslyn;

namespace Overmock.Compilation
{
	public class OvermockSetup
	{
		public static void UseIlBuilder()
		{
			OvermockBuilder.UseBuilder(new ILOvermockBuilder());
		}

		public static void UseRoslynBuilder()
		{
			OvermockBuilder.UseBuilder(new RoslynOvermockBuilder());
		}

		public static void UseCodeDomBuilder()
		{
			OvermockBuilder.UseBuilder(new RoslynOvermockBuilder());
		}
	}
}
