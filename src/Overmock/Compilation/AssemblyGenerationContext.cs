using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Overmock.Compilation
{
    /// <summary>
    /// 
    /// </summary>
    public class AssemblyGenerationContext
    {
        private readonly List<MemberDeclarationSyntax> _members = new List<MemberDeclarationSyntax>();
        private readonly List<string> _namespaces = new List<string>
        {
            "System",
            "System.Threading.Tasks",
            "System.Collections.Generic",
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public AssemblyGenerationContext(IOvermock target)
        {
            Target = target;
            TargetType = target.Type;
            
            if (target.Type.Namespace != null)
            {
                _namespaces.Add(target.Type.Namespace);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        /// 
        /// </summary>
        public IOvermock Target { get; }

        /// <summary>
        /// 
        /// </summary>
        public ClassDeclarationSyntax? ClassDeclaration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public NamespaceDeclarationSyntax? NamespaceDeclaration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="members"></param>
        /// <returns></returns>
        public AssemblyGenerationContext AddMembers(params MemberDeclarationSyntax[] members)
        {
            _members.AddRange(members);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namespace"></param>
        /// <returns></returns>
        public AssemblyGenerationContext AddNamespace(string? @namespace)
        {
            if (!string.IsNullOrWhiteSpace(@namespace) && !_namespaces.Contains(@namespace))
            {
                _namespaces.Add(@namespace);
            }

            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<string> GetNamespaces()
        {
            return _namespaces.ToImmutableArray();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="namespaceDeclaration"></param>
        /// <returns></returns>
        public AssemblyGenerationContext SetNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            NamespaceDeclaration = namespaceDeclaration;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classDeclaration"></param>
        /// <returns></returns>
        public AssemblyGenerationContext SetClassDeclaration(ClassDeclarationSyntax classDeclaration)
        {
            ClassDeclaration = classDeclaration;
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
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