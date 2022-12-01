using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Overmock.Generation
{
    public class AssemblyGenerationContext
    {
        private readonly List<MemberDeclarationSyntax> _members = new List<MemberDeclarationSyntax>();
        private readonly List<string> _namespaces = new List<string>
        {
            "System",
            "System.Threading.Tasks",
            "System.Collections.Generic",
        };

        public AssemblyGenerationContext(IOvermock target)
        {
            Target = target;
            TargetType = target.Type;
            
            if (target.Type.Namespace != null)
            {
                _namespaces.Add(target.Type.Namespace);
            }
        }

        public Type TargetType { get; }

        public IOvermock Target { get; }

        public ClassDeclarationSyntax? ClassDeclaration { get; private set; }

        public NamespaceDeclarationSyntax? NamespaceDeclaration { get; private set; }

        public AssemblyGenerationContext AddMembers(params MemberDeclarationSyntax[] members)
        {
            _members.AddRange(members);
            return this;
        }

        public AssemblyGenerationContext AddNamespace(string? @namespace)
        {
            if (!string.IsNullOrWhiteSpace(@namespace) && !_namespaces.Contains(@namespace))
            {
                _namespaces.Add(@namespace);
            }

            return this;
        }

        public ImmutableArray<string> GetNamespaces()
        {
            return _namespaces.ToImmutableArray();
        }

        public AssemblyGenerationContext SetNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            NamespaceDeclaration = namespaceDeclaration;
            return this;
        }

        public AssemblyGenerationContext SetClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            ClassDeclaration = classDeclaration;
            return this;
        }

        public CompilationUnitSyntax BuildCompilationUnit()
        {
            Ex.Throw.If.BuildComponentsAreNull(this);

            var namespaceDeclaration = NamespaceDeclaration!.AddUsings(_namespaces.Select(n =>
                    SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(n))
                ).ToArray());

            var classDeclaration = ClassDeclaration!.AddMembers(_members.ToArray());

            return SyntaxFactory.CompilationUnit().AddMembers(
                namespaceDeclaration!.AddMembers(
                        classDeclaration.NormalizeWhitespace())
                    .NormalizeWhitespace());
        }
    }
}