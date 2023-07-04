namespace Overmock.Compilation.IL
{
	/// <summary>
	/// 
	/// </summary>
	public class ILOvermockBuilder : IOvermockBuilder
	{
		ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider)
		{
			return new ILTypeBuilder(new IlAssemblyCompiler(new IlOvermockMethodBuilder()), argsProvider);
		}
	}
}
