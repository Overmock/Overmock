using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Overmock.Compilation
{
    internal class OvermockTypeBuilder : ITypeBuilder
    {
        private readonly IAssemblyGenerator _assemblyGenerator;
        private readonly Action<SetupArgs>? _constructorArguments;

        internal OvermockTypeBuilder(IAssemblyGenerator generator, Action<SetupArgs>? argsProvider = null)
        {
            _assemblyGenerator = generator;
            _constructorArguments = argsProvider;
        }

        public T? BuildType<T>(IOvermock<T> target) where T : class
        {
            if (target.GetCompiledType() == null)
            {
                var generationContext = _assemblyGenerator.GetClassDeclaration(target);

                var compilationUnit = generationContext.BuildCompilationUnit();

                var compiler = _assemblyGenerator.CompileAssembly(generationContext, compilationUnit);

                using (var stream = new MemoryStream())
                {
                    var result = compiler.Emit(stream);

                    if (!result.Success)
                    {
                        throw new OvermockException(result.Diagnostics.ToString());
                    }

                    var assembly = AppDomain.CurrentDomain.Load(stream.ToArray());

                    target.SetCompiledType(assembly);

                    if (target.GetCompiledType() == null)
                    {
                        throw new OvermockException($"Failed to generate type for: '{target.TypeName}'");
                    }
                }
            }

            if (_constructorArguments != null)
            {
                var args = new SetupArgs();
                _constructorArguments.Invoke(args);

                return Activator.CreateInstance(target.GetCompiledType()!, args.Parameters) as T;
            }

            return Activator.CreateInstance(target.GetCompiledType()!) as T;
        }
    }
}