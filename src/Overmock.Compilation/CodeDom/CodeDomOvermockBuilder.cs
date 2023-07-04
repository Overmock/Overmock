using System;

namespace Overmock.Compilation.CodeDom
{
	/// <summary>
	/// 
	/// </summary>
	public class CodeDomOvermockBuilder : IOvermockBuilder
	{
		ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider)
		{
			return new CodeDomTypeBuilder(argsProvider);
		}
	}
}
