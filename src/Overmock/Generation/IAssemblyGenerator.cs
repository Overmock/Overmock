using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace Overmock.Generation
{
    public interface IAssemblyGenerator
    {
        CSharpCompilation CompileAssembly<T>(IOvermock<T> target, CompilationUnitSyntax compilation, ImmutableArray<string> namespaces) where T : class;

        NamespaceDeclarationSyntax GetNamespaceDeclarations<T>(IOvermock<T> target) where T : class;

        IEnumerable<MetadataReference> LoadAssemblyReferenceses<T>(IOvermock<T> target) where T : class;

        ClassDeclarationSyntax GetClassDeclaration<T>(IOvermock<T> target) where T : class;

        ClassDeclarationSyntax ImplementatInterfaces<T>(IOvermock<T> target, ClassDeclarationSyntax classDeclaration) where T : class;
    }
}