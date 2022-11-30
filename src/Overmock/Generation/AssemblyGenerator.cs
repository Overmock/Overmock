using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.Collections.Immutable;
using System.Reflection;
using System.Text;

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
                classDeclaration = ImplementInterface(target, interfaceType, classDeclaration);
            }

            return ImplementInterface(target, target.Type, classDeclaration);
        }

        private ClassDeclarationSyntax ImplementInterface<T>(IOvermock<T> target, Type interfaceType, ClassDeclarationSyntax classDeclaration) where T : class
        {
            // TODO: Mock properties
            var overmockedProperties = target.GetOvermockedProperties();
            var interfaceProperties = interfaceType.GetProperties().Except(overmockedProperties.Select(p => (PropertyInfo)p.Expression.Member));

            foreach (var property in target.GetOvermockedProperties())
            {
                classDeclaration = BuildOvermockedProperty(target, property, classDeclaration);
            }

            // Add properties
            foreach (var property in interfaceProperties)
            {
                var propertyDeclaration = BuildProperty(interfaceType, property);
                classDeclaration = classDeclaration.AddMembers(propertyDeclaration);
            }

            // Add overmock methods
            var overmockedMethods = target.GetOvermockedMethods();
            var interfaceMethods = interfaceType.GetMethods().Except(overmockedMethods.Select(m => m.Expression.Method)).Where(m => !m.IsSpecialName);

            foreach (var method in overmockedMethods)
            {
                classDeclaration = BuildOvermockedMethod(target, method, classDeclaration);
            }

            // Add other interface methods
            foreach (var method in interfaceMethods)
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

            foreach (var property in target.GetOvermockedProperties())
            {
                result = BuildOvermockedProperty(target, property, result);
            }

            foreach (var method in target.GetOvermockedMethods())
            {
                result = BuildOvermockedMethod(target, method, result);
            }

            return result.NormalizeWhitespace();
        }

        private ClassDeclarationSyntax BuildOvermockedMethod<T>(IOvermock<T> target, IMethodCall method, ClassDeclarationSyntax result) where T : class
        {
            var overrides = method.GetOverrides();

            if (overrides.Any())
            {
                var methodOvermocked = BuildMethodSignature(method.Expression.Method);

                var exception = overrides.SingleOrDefault(o => o.Exception != null);

                if (exception != default)
                {
                    var exceptionType = exception.Exception.GetType();

                    return result.AddMembers(methodOvermocked.WithBody(
                        sf.Block(sf.ParseStatement($"throw new {exceptionType.Namespace}.{exceptionType.Name}(\"{exception.Exception.Message}\");"))
                    ));
                }

                result = result.AddMembers(methodOvermocked.WithBody(NotImplementedExceptionMethodBody));
            }

            return result;
        }

        private ClassDeclarationSyntax BuildOvermockedProperty<T>(IOvermock<T> target, IPropertyCall property, ClassDeclarationSyntax result) where T : class
        {
            var overrides = property.GetOverrides();

            if (overrides.Any())
            {
                var propertyInfo = (PropertyInfo)property.Expression.Member;
                var propertyDeclaration = sf.PropertyDeclaration(GetSafeTypeName(propertyInfo.PropertyType), propertyInfo.Name)
                    .AddModifiers(sf.Token(SyntaxKind.PublicKeyword));

                var exception = overrides.SingleOrDefault(o => o.Exception != null);

                if (exception != default)
                {
                    var exceptionType = exception.Exception.GetType();

                    return result.AddMembers(propertyDeclaration.AddAccessorListAccessors(
                        sf.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                            .WithBody(sf.Block(sf.ParseStatement($"throw new {exceptionType.Namespace}.{exceptionType.Name}(\"{exception.Exception.Message}\");"))),
                        sf.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                            .WithBody(NotImplementedExceptionMethodBody)
                        ));
                }

                result = result.AddMembers(propertyDeclaration.AddAccessorListAccessors(
                    sf.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration)
                        .WithBody(NotImplementedExceptionMethodBody),
                    sf.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration)
                        .WithBody(NotImplementedExceptionMethodBody)
                ));
            }

            return result;
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
            TypeSyntax returnType = GetSafeTypeName(method.ReturnParameter.ParameterType);
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

        private static IEnumerable<ParameterSyntax> BuildParameterList(IEnumerable<ParameterInfo> parameters)
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

        private static TypeSyntax GetSafeTypeName(Type type)
        {
            // TODO: Handle Generic typed returns..they're type names are nasty..
            if (!type.IsGenericType)
            {
                return sf.ParseTypeName(type.Name ?? "void");
            }


            return sf.ParseTypeName(
                ParseGenericTypeName(type)
            );
        }

        private static string ParseGenericTypeName(Type type)
        {
            const char open = '<';
            const char close = '>';
            const char tilde = '`';
            const char comma = ',';

            var result = new StringBuilder(type.Name[..type.Name.IndexOf(tilde)])
                .Append(open);

            foreach (var genericParameter in type.GetGenericArguments())
            {
                result.Append(genericParameter.IsGenericType
                    ? ParseGenericTypeName(genericParameter)
                    : genericParameter.Name);

                result.Append(comma);
            }

            result = result.Remove(result.Length - 1, 1);

            return result.Append(close).ToString();
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

            var result = sf.PropertyDeclaration(GetSafeTypeName(property.PropertyType), sf.Identifier(property.Name));

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
