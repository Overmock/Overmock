using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Overmock.Generation
{
    public interface IAssemblyGenerator
    {
        CSharpCompilation CompileAssembly(AssemblyGenerationContext context, CompilationUnitSyntax compilation);

        NamespaceDeclarationSyntax GetNamespaceDeclaration(AssemblyGenerationContext context);

        AssemblyGenerationContext GetClassDeclaration(IOvermock overmock);
    }
}