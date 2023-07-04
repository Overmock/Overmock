using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Overmock.Compilation.Roslyn
{

	/// <summary>
	/// 
	/// </summary>
	public interface IRoslynAssemblyGenerator
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <param name="compilation"></param>
		/// <returns></returns>
		CSharpCompilation GenerateCompilation(RoslynAssemblyGenerationContext context, CompilationUnitSyntax compilation);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		NamespaceDeclarationSyntax GetNamespaceDeclaration(RoslynAssemblyGenerationContext context);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		void GetClassDeclaration(RoslynAssemblyGenerationContext context);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		CompilationUnitSyntax BuildCompilationUnit(RoslynAssemblyGenerationContext context);

		///// <summary>
		///// 
		///// </summary>
		///// <param name="namespaceToAdd"></param>
		///// <returns></returns>
		//void AddNamespace(string? @namespaceToAdd);
	}
}