using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;

namespace Overmock.Compilation.Roslyn
{
	/// <summary>
	/// 
	/// </summary>
	public class RoslynAssemblyGenerationContext : IAssemblyCompilerContext
	{
		private readonly List<MemberDeclarationSyntax> _members = new List<MemberDeclarationSyntax>();


		/// <summary>
		/// 
		/// </summary>
		/// <param name="target"></param>
		public RoslynAssemblyGenerationContext(IOvermock target)
		{
			Target = target;
			TargetType = target.Type;
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
		public RoslynAssemblyGenerationContext AddMembers(params MemberDeclarationSyntax[] members)
		{
			_members.AddRange(members);
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="namespaceDeclaration"></param>
		/// <returns></returns>
		public RoslynAssemblyGenerationContext SetNamespaceDeclaration(NamespaceDeclarationSyntax namespaceDeclaration)
		{
			NamespaceDeclaration = namespaceDeclaration;
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="classDeclaration"></param>
		/// <returns></returns>
		public RoslynAssemblyGenerationContext SetClassDeclaration(ClassDeclarationSyntax classDeclaration)
		{
			ClassDeclaration = classDeclaration;
			return this;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ClassDeclarationSyntax GetClassDeclaration()
		{
			return ClassDeclaration = ClassDeclaration!.AddMembers(_members.ToArray());
		}
	}
}