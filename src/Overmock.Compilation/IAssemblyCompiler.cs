using System.Reflection;

namespace Overmock.Compilation
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAssemblyCompiler<in TContext> where TContext : IAssemblyCompilerContext
	{
        /// <summary>
        /// 
        /// </summary>
        IOvermockMethodBuilder MethodBuilder { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Assembly CompileAssembly(TContext context);
	}
}