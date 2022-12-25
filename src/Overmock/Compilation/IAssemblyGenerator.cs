using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Overmock.Compilation
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAssemblyGenerator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="compilation"></param>
        /// <returns></returns>
        CSharpCompilation CompileAssembly(AssemblyGenerationContext context, CompilationUnitSyntax compilation);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        NamespaceDeclarationSyntax GetNamespaceDeclaration(AssemblyGenerationContext context);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="overmock"></param>
        /// <returns></returns>
        AssemblyGenerationContext GetClassDeclaration(IOvermock overmock);
    }
}