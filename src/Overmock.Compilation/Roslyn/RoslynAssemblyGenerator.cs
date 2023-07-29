using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Overmock.Mocking;
using Overmock.Mocking.Internal;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Compilation.Roslyn
{
	// INFO: I really don't like this but it is what it is.
	using sf = SyntaxFactory;

	internal class RoslynAssemblyGenerator : IRoslynAssemblyGenerator
	{
		private static readonly BlockSyntax _notImplementedExceptionMethodBody = sf.Block(
			sf.ParseStatement("throw new NotImplementedException();")
		);

		private readonly List<string> _namespaces = new List<string>
		{
			"System",
			"System.Threading.Tasks",
			"System.Collections.Generic",
		};

		private static readonly IEnumerable<MetadataReference> _defaultReferences = new[]
		{
			MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
			MetadataReference.CreateFromFile(typeof(Task).GetTypeInfo().Assembly.Location),
			MetadataReference.CreateFromFile(typeof(IList<>).GetTypeInfo().Assembly.Location)
		};

		public ImmutableArray<string> GetNamespaces()
		{
			return _namespaces.ToImmutableArray();
		}

		public static IEnumerable<MetadataReference> LoadAssemblyReferences(RoslynAssemblyGenerationContext context)
		{
			var references = _defaultReferences.Append(MetadataReference.CreateFromFile(context.Target.Type.Assembly.Location)).ToList();

			references.AddRange(context.Target.Type.Assembly.GetReferencedAssemblies().Select(assemblyName => {
				var path = Path.Combine(Assembly.Load(assemblyName).Location);
				return MetadataReference.CreateFromFile(path);
			}));

			return references.ToArray();

		}

		private static BlockSyntax BuildConstructorBody(ConstructorInfo constructor, RoslynAssemblyGenerationContext context)
		{
			return sf.Block();
		}

		private static T BuildParameters<T>(ParameterInfo[] parameters, T result) where T : BaseMethodDeclarationSyntax
		{
			if (parameters.Any())
			{
				result = (T)result.AddParameterListParameters(
					BuildParameterList(parameters).ToArray()
				);
			}

			return result;
		}

		private static IEnumerable<ParameterSyntax> BuildParameterList(IEnumerable<ParameterInfo> parameters)
		{
			var result = new List<ParameterSyntax>();

			foreach (var parameter in parameters)
			{
				result.Add(sf.Parameter(
					sf.Identifier(parameter.Name!))
						.WithType(sf.ParseTypeName(parameter.ParameterType.Name))
						.NormalizeWhitespace()
				);
			}

			return result;
		}

		public void GetClassDeclaration(RoslynAssemblyGenerationContext context)
		{
			if (context.TargetType.Namespace != null && !_namespaces.Contains(context.TargetType.Namespace))
			{
				_namespaces.Add(context.TargetType.Namespace);
			}

			context.SetClassDeclaration(sf.ClassDeclaration(context.Target.TypeName)
				.AddModifiers(sf.Token(SyntaxKind.PublicKeyword))
				.AddBaseListTypes(
					sf.SimpleBaseType(sf.ParseTypeName(context.TargetType.Name))
				));

			if (context.TargetType.IsInterface)
			{
				ImplementInterfaces(context);
			}

			if (context.TargetType.IsClass && !context.TargetType.IsSealed)
			{
				InheritClass(context);
			}

			context.SetNamespaceDeclaration(GetNamespaceDeclaration(context));
		}

		public CSharpCompilation GenerateCompilation(RoslynAssemblyGenerationContext context, CompilationUnitSyntax compilation)
		{
			return CSharpCompilation.Create($"{context.Target.TypeName}.dll",
				syntaxTrees: new[] { compilation.SyntaxTree },
				references: LoadAssemblyReferences(context),
				options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, metadataImportOptions: MetadataImportOptions.All)
					.WithOptimizationLevel(OptimizationLevel.Release)
					.WithOverflowChecks(true)
					.WithUsings(GetNamespaces())
			);
		}

		public CompilationUnitSyntax BuildCompilationUnit(RoslynAssemblyGenerationContext context)
		{
			var namespaceDeclaration = context.NamespaceDeclaration!.AddUsings(_namespaces.Select(n =>
					sf.UsingDirective(sf.ParseName(n))
				).ToArray());

			var classDeclaration = context.GetClassDeclaration();

			return sf.CompilationUnit().AddMembers(
				namespaceDeclaration!.AddMembers(
						classDeclaration.NormalizeWhitespace())
					.NormalizeWhitespace());
		}

		private void AddNamespace(string? @namespace)
		{
			if (!string.IsNullOrWhiteSpace(@namespace) && !_namespaces.Contains(@namespace))
			{
				_namespaces.Add(@namespace);
			}
		}

		public NamespaceDeclarationSyntax GetNamespaceDeclaration(RoslynAssemblyGenerationContext context)
		{
			return sf.NamespaceDeclaration(sf.ParseName("OvermockGenerated"));
		}

		public void ImplementInterfaces(RoslynAssemblyGenerationContext context)
		{
			if (context.Target.Type.IsInterface)
			{
				foreach (var interfaceType in context.Target.Type.GetInterfaces())
				{
					ImplementInterface(context, interfaceType);
				}

				ImplementInterface(context, context.Target.Type);
			}
		}

		private void ImplementInterface(RoslynAssemblyGenerationContext context, Type interfaceType)
		{
			// TODO: Mock properties
			var overmockedProperties = context.Target.GetOvermockedProperties().ToArray();
			var interfaceProperties = interfaceType.GetProperties().Except(overmockedProperties.Select(p => (PropertyInfo)p.Expression.Member));

			foreach (var property in overmockedProperties)
			{
				BuildOvermockedProperty(property, context);
			}

			// Add properties
			foreach (var property in interfaceProperties)
			{
				BuildProperty(context, interfaceType, property);
			}

			// Add overmock methods
			var overmockedMethods = context.Target.GetOvermockedMethods().ToArray();
			var interfaceMethods = interfaceType.GetMethods().Except(overmockedMethods.Select(m => m.Expression.Method)).Where(m => !m.IsSpecialName);

			foreach (var method in overmockedMethods)
			{
				BuildOvermockedMethod(method, context);
			}

			// Add other interface methods
			foreach (var method in interfaceMethods)
			{
				var methodDeclaration = BuildMethodSignature(method, context)
					// TODO: Add method wrappers where needed.
					.WithBody(_notImplementedExceptionMethodBody);

				context.AddMembers(methodDeclaration);
			}
		}

		private void InheritClass(RoslynAssemblyGenerationContext context)
		{
			foreach (var constructor in context.Target.Type.GetConstructors())
			{
				var constructorDeclaration = BuildConstructorSignature(constructor, context);

				context.AddMembers(constructorDeclaration
					.WithBody(BuildConstructorBody(constructor, context))
				);
			}

			foreach (var property in context.Target.GetOvermockedProperties())
			{
				BuildOvermockedProperty(property, context);
			}

			foreach (var method in context.Target.GetOvermockedMethods())
			{
				BuildOvermockedMethod(method, context);
			}
		}

		private void BuildOvermockedMethod(IMethodCall method, RoslynAssemblyGenerationContext context)
		{
			var overrides = method.GetOverrides().ToArray();

			if (overrides.Any())
			{
				var methodOvermocked = BuildMethodSignature(method.Expression.Method, context);

				var exception = (ThrowExceptionOverride)overrides.SingleOrDefault(o => o is ThrowExceptionOverride)!;

				if (exception != default)
				{
					var exceptionType = exception.Exception.GetType();

					context.AddMembers(methodOvermocked.WithBody(
						sf.Block(sf.ParseStatement($"throw new {exceptionType.Namespace}.{exceptionType.Name}(\"{exception.Exception.Message}\");"))
					));
					return;
				}

				context.AddMembers(methodOvermocked.WithBody(_notImplementedExceptionMethodBody));
			}
		}

		private void BuildOvermockedProperty(IPropertyCall property, RoslynAssemblyGenerationContext context)
		{
			var overrides = property.GetOverrides();

			if (overrides.Any())
			{
				var propertyInfo = (PropertyInfo)property.Expression.Member;
				var propertyDeclaration = sf.PropertyDeclaration(GetSafeTypeName(propertyInfo.PropertyType, context), propertyInfo.Name)
					.AddModifiers(sf.Token(SyntaxKind.PublicKeyword));

				var exception = (ThrowExceptionOverride)overrides.SingleOrDefault(o => o is ThrowExceptionOverride);

				if (exception != default)
				{
					var exceptionType = exception.Exception.GetType();

					context.AddMembers(propertyDeclaration.AddAccessorListAccessors(
						sf.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
							.WithBody(sf.Block(sf.ParseStatement($"throw new {exceptionType.Namespace}.{exceptionType.Name}(\"{exception.Exception.Message}\");"))),
						sf.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
							.WithBody(_notImplementedExceptionMethodBody)
						));

					return;
				}

				context.AddMembers(propertyDeclaration.AddAccessorListAccessors(
					sf.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
						.WithBody(_notImplementedExceptionMethodBody),
					sf.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
						.WithBody(_notImplementedExceptionMethodBody)
				));
			}
		}

		private ConstructorDeclarationSyntax BuildConstructorSignature(MethodBase constructor, RoslynAssemblyGenerationContext context)
		{
			var parameters = constructor.GetParameters();
			var methodDeclaration = sf.ConstructorDeclaration(sf.Identifier(context.Target.TypeName))
				.AddModifiers(sf.Token(SyntaxKind.PublicKeyword));

			if (parameters.Any())
			{
				methodDeclaration = BuildBaseInitializer(context, parameters, methodDeclaration);
			}

			return BuildParameters(parameters, methodDeclaration);
		}

		private ConstructorDeclarationSyntax BuildBaseInitializer(RoslynAssemblyGenerationContext context, IEnumerable<ParameterInfo> parameters, ConstructorDeclarationSyntax methodDeclaration)
		{
			var arguments = new SeparatedSyntaxList<ArgumentSyntax>().AddRange(parameters.Select(p => {
				AddNamespace(p.ParameterType.Namespace);
				return sf.Argument(sf.IdentifierName(p.Name!));
			}));

			return methodDeclaration.WithInitializer(
				sf.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
				.AddArgumentListArguments(arguments.ToArray())
			);
		}

		private MethodDeclarationSyntax BuildMethodSignature(MethodInfo method, RoslynAssemblyGenerationContext context)
		{
			TypeSyntax returnType = GetSafeTypeName(method.ReturnParameter.ParameterType, context);
			var result = sf.MethodDeclaration(returnType, method.Name)
				.AddModifiers(method.IsPublic ? sf.Token(SyntaxKind.PublicKeyword) : sf.Token(SyntaxKind.InternalKeyword));

			if (!method.DeclaringType.IsInterface)
			{
				if (method.IsVirtual)
				{
					result = result.AddModifiers(sf.Token(SyntaxKind.OverrideKeyword));
				}
				else
				{
					result = result.AddModifiers(sf.Token(SyntaxKind.NewKeyword));
				}
			}

			return BuildParameters(method.GetParameters(), result);
		}

		private TypeSyntax GetSafeTypeName(Type type, RoslynAssemblyGenerationContext context)
		{
			AddNamespace(type.Namespace);

			if (!type.IsGenericType)
			{
				return sf.ParseTypeName(type == typeof(void) ? "void" : type.Name);
			}

			return sf.ParseTypeName(
				ParseGenericTypeName(type, context)
			);
		}

		private string ParseGenericTypeName(Type type, RoslynAssemblyGenerationContext context)
		{
			const char open = '<';
			const char close = '>';
			const char tilde = '`';
			const char comma = ',';

			var result = new StringBuilder(type.Name[..type.Name.IndexOf(tilde)])
				.Append(open);

			foreach (var genericParameter in type.GetGenericArguments())
			{
				result.Append(GetSafeTypeName(genericParameter, context));

				result.Append(comma);
			}

			result = result.Remove(result.Length - 1, 1);

			return result.Append(close).ToString();
		}

		private void BuildProperty(RoslynAssemblyGenerationContext context, Type type, PropertyInfo property, bool implementExplicitlyIfInterface = false)
		{
			var accessorList = new List<AccessorDeclarationSyntax>();

			// Add getter
			if (property.CanRead)
			{
				accessorList.Add(sf.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
					.WithSemicolonToken(sf.Token(SyntaxKind.SemicolonToken)));
			}

			// Add setter
			if (property.CanWrite)
			{
				accessorList.Add(sf.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
					.WithSemicolonToken(sf.Token(SyntaxKind.SemicolonToken)));
			}

			var result = sf.PropertyDeclaration(GetSafeTypeName(property.PropertyType, context), sf.Identifier(property.Name));

			if (type.IsInterface && implementExplicitlyIfInterface)
			{
				result = result.WithExplicitInterfaceSpecifier(
					sf.ExplicitInterfaceSpecifier(sf.ParseName(type.Name), sf.Token(SyntaxKind.DotToken))
				);
			}
			else
			{
				result = result.AddModifiers(sf.Token(SyntaxKind.PublicKeyword));
			}

			context.AddMembers(result.AddAccessorListAccessors(accessorList.ToArray()));
		}
	}
}
