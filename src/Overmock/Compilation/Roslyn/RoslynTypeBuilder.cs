using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Overmock.Compilation.Roslyn
{
    internal class RoslynTypeBuilder : ITypeBuilder
    {
        private readonly IRoslynAssemblyGenerator _assemblyGenerator;
        private readonly Action<SetupArgs>? _constructorArguments;

        internal RoslynTypeBuilder(IRoslynAssemblyGenerator assemblyGenerator, Action<SetupArgs>? argsProvider = null)
        {
            _assemblyGenerator = assemblyGenerator;
            _constructorArguments = argsProvider;
        }

        public T? BuildType<T>(IOvermock<T> target) where T : class
        {
            if (target.GetCompiledType() == null)
            {
                var context = RoslynAssemblyCompiler.GetAssemblyCompilerContext(target);

                _assemblyGenerator.GetClassDeclaration(context);

                var compilationUnit = _assemblyGenerator.BuildCompilationUnit(context);

                var csharpCompiler = _assemblyGenerator.GenerateCompilation(context, compilationUnit);

                var compiler = new RoslynAssemblyCompiler(csharpCompiler);

                var assembly = compiler.CompileAssembly(context);

                target.SetCompiledType(assembly);

                if (target.GetCompiledType() == null)
                {
                    throw new OvermockException($"Failed to generate type for: '{target.TypeName}'");
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

        private class RoslynAssemblyCompiler : AssemblyCompiler<RoslynAssemblyGenerationContext>
        {
			private readonly CSharpCompilation _csharpCompiler;

			public RoslynAssemblyCompiler(CSharpCompilation csharpCompiler)
            {
                _csharpCompiler = csharpCompiler;
            }

            public override Assembly CompileAssembly(RoslynAssemblyGenerationContext context)
            {
                using (var stream = new MemoryStream())
                {
                    var result = _csharpCompiler.Emit(stream);

                    if (!result.Success)
                    {
                        throw new OvermockException(result.Diagnostics.ToString());
                    }

                    return AppDomain.CurrentDomain.Load(stream.ToArray());
                }
            }

            public static RoslynAssemblyGenerationContext GetAssemblyCompilerContext(IOvermock target)
            {
                return new RoslynAssemblyGenerationContext(target);
            }
        }
    }
}