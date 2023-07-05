using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Overmock.Compilation.IL
{
    /// <inheritdoc />
    public class IlAssemblyCompiler : IAssemblyCompiler<IlAssemblyCompilerContext>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="methodBuilder"></param>
        public IlAssemblyCompiler(IlOvermockMethodBuilder methodBuilder)
        {
            MethodBuilder = methodBuilder;
        }

        /// <inheritdoc/>
        public IOvermockMethodBuilder MethodBuilder { get; init; }

        /// <inheritdoc/>
        public Assembly CompileAssembly(IlAssemblyCompilerContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <inheritdoc />
    public class IlAssemblyCompilerContext : AssemblyCompilerContextBase
    {
        /// <inheritdoc />
        public IlAssemblyCompilerContext(IOvermock target) : base(target)
        {
        }
    }

    public class IlOvermockMethodBuilder : IOvermockMethodBuilder
    {
        public void BuildMethod(MethodInfo method)
        {
            throw new NotImplementedException();
        }

        public void BuildConstructor(ConstructorInfo? constructor = default)
        {
            throw new NotImplementedException();
        }
    }
}
