namespace Overmock.Compilation.IL
{
	/// <summary>
	/// 
	/// </summary>
	public class ILOvermockBuilder : IOvermockBuilder
	{
		ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider)
		{
			return new ILTypeBuilder(argsProvider);
		}
	}
}
