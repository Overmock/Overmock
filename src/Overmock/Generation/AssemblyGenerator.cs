using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Reflection;

namespace Overmock.Generation
{
    // INFO: I really don't like this but it is what it is.
    using sf = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

    internal class AssemblyGenerator : IAssemblyGenerator
    {
        private static readonly BlockSyntax NotImplementedExceptionMethodBody = sf.Block(
            sf.ParseStatement("throw new NotImplementedException();")
        );

        private static readonly IEnumerable<MetadataReference> DefaultReferences = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Task).GetTypeInfo().Assembly.Location),
            MetadataReference.CreateFromFile(typeof(IList<>).GetTypeInfo().Assembly.Location)
        };

        internal static readonly IAssemblyGenerator Instance = new AssemblyGenerator();

        public CSharpCompilation CompileAssembly<T>(IOvermock<T> target, CompilationUnitSyntax compilation, ImmutableArray<string> namespaces) where T : class
        {
            return CSharpCompilation.Create($"{target.TypeName}.dll",
                syntaxTrees: new[] { compilation.SyntaxTree },
                references: LoadAssemblyReferenceses(target),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, metadataImportOptions: MetadataImportOptions.All)
                    .WithOptimizationLevel(OptimizationLevel.Release)
                    .WithOverflowChecks(true)
                    .WithUsings(namespaces)
            );
        }

        public NamespaceDeclarationSyntax GetNamespaceDeclarations<T>(IOvermock<T> target) where T : class
        {
            return sf.NamespaceDeclaration(sf.ParseName("OvermockGenerated"))
                .AddUsings(
                    sf.UsingDirective(sf.ParseName("System")),
                    sf.UsingDirective(sf.ParseName("System.Threading.Tasks")),
                    sf.UsingDirective(sf.ParseName("System.Collections.Generic")),
                    sf.UsingDirective(sf.ParseName(target.Type.Namespace))
                );
        }

        public IEnumerable<MetadataReference> LoadAssemblyReferenceses<T>(IOvermock<T> target) where T : class
        {
            var references = DefaultReferences.Append(MetadataReference.CreateFromFile(target.Type.Assembly.Location)).ToList();

            references.AddRange(target.Type.Assembly.GetReferencedAssemblies().Select(assemblyName =>
            {
                var path = Path.Combine(Assembly.Load(assemblyName).Location);
                return MetadataReference.CreateFromFile(path);
            }));

            return references.ToArray();

        }

        public ClassDeclarationSyntax GetClassDeclaration<T>(IOvermock<T> target) where T : class
        {
            var type = target.Type;
            var result = sf.ClassDeclaration(target.TypeName)
                .AddModifiers(sf.Token(SyntaxKind.PublicKeyword))
                .AddBaseListTypes(
                    sf.SimpleBaseType(sf.ParseTypeName(type.Name))
                );

            if (type.IsInterface)
            {
                return ImplementatInterfaces(target, result);
            }

            if (type.IsClass && !type.IsSealed)
            {
                return InheritClass(target, result);
            }

            // TODO: handle other types

            return result;
        }

        public ClassDeclarationSyntax ImplementatInterfaces<T>(IOvermock<T> target, ClassDeclarationSyntax classDeclaration) where T : class
        {
            foreach (var interfaceType in target.Type.GetInterfaces())
            {
                classDeclaration = ImplementInterface(interfaceType, classDeclaration);
            }

            return ImplementInterface(target.Type, classDeclaration);
        }

        private ClassDeclarationSyntax ImplementInterface(Type interfaceType, ClassDeclarationSyntax classDeclaration)
        {
            // Add properties
            foreach (var property in interfaceType.GetProperties().Where(m => !m.IsSpecialName))
            {
                var propertyDeclaration = BuildProperty(interfaceType, property);
                classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
            }

            // Add methods
            foreach (var method in interfaceType.GetMethods().Where(m => !m.IsSpecialName))
            {
                var methodDeclaration = BuildMethodSignature(method)
                    // TODO: Add method wrappers where needed.
                    .WithBody(NotImplementedExceptionMethodBody);

                classDeclaration = classDeclaration.AddMembers(methodDeclaration);
            }

            return classDeclaration;
        }

        private ClassDeclarationSyntax InheritClass<T>(IOvermock<T> target, ClassDeclarationSyntax result) where T : class
        {
            var constructors = new List<ConstructorDeclarationSyntax>();

            foreach (var constructor in target.Type.GetConstructors())
            {
                var constructorDeclaration = BuildConstructorSignature(target, constructor);

                result = result.AddMembers(constructorDeclaration
                    .WithBody(BuildConstructorBody(target, constructor))
                );
            }

            //foreach (MethodInfo method in target.GetMethods())
            //{

            //}

            //foreach (var property in target.GetProperties())
            //{

            //}
            // TODO: methods/properties

            return result.NormalizeWhitespace();
        }

        private BlockSyntax BuildConstructorBody<T>(IOvermock<T> target, ConstructorInfo constructor) where T : class
        {
            return sf.Block();
        }

        private ConstructorDeclarationSyntax BuildConstructorSignature<T>(IOvermock<T> target, ConstructorInfo constructor) where T : class
        {
            var parameters = constructor.GetParameters();
            var methodDeclaration = sf.ConstructorDeclaration(sf.Identifier(target.TypeName))
                .AddModifiers(sf.Token(SyntaxKind.PublicKeyword));

            if (parameters.Any())
            {
                methodDeclaration = BuildBaseInitializer(parameters, methodDeclaration);
            }

            return BuildParameters(parameters, methodDeclaration);
        }

        private ConstructorDeclarationSyntax BuildBaseInitializer(IEnumerable<ParameterInfo> parameters, ConstructorDeclarationSyntax methodDeclaration)
        {
            var arguments = new SeparatedSyntaxList<ArgumentSyntax>().AddRange(
                parameters.Select(p => sf.Argument(sf.IdentifierName(p.Name)))
            );

            return methodDeclaration.WithInitializer(
                sf.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                .AddArgumentListArguments(arguments.ToArray())
            );
        }

        private MethodDeclarationSyntax BuildMethodSignature(MethodInfo method)
        {
            // TODO: Handle Generic typed returns..they're type names are nasty..
            var returnType = sf.ParseTypeName(method.ReturnParameter.ParameterType.Name ?? "void");
            var result = sf.MethodDeclaration(returnType, method.Name)
                .AddModifiers(sf.Token(SyntaxKind.PublicKeyword));
            return BuildParameters(method.GetParameters(), result);
        }

        private T BuildParameters<T>(IEnumerable<ParameterInfo> parameters, T result) where T : BaseMethodDeclarationSyntax
        {
            if (parameters.Any())
            {
                result = (T)result.AddParameterListParameters(
                    BuildParameterList(parameters).ToArray()
                );
            }

            return result;
        }

        private IEnumerable<ParameterSyntax> BuildParameterList(IEnumerable<ParameterInfo> parameters)
        {
            var result = new List<ParameterSyntax>();

            foreach (var parameter in parameters)
            {
                result.Add(sf.Parameter(
                    sf.Identifier(parameter.Name))
                        .WithType(sf.ParseTypeName(parameter.ParameterType.Name))
                        .NormalizeWhitespace()
                );
            }

            return result;
        }

        private static PropertyDeclarationSyntax BuildProperty(Type type, PropertyInfo property, bool implementExplicitlyIfInterface = false)
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

            var result = sf.PropertyDeclaration(sf.ParseTypeName(property.PropertyType.Name), sf.Identifier(property.Name));

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

            return result.AddAccessorListAccessors(accessorList.ToArray())
                .NormalizeWhitespace();
        }
    }
}
