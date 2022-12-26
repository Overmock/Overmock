using System.Reflection;

namespace Overmock.Compilation
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAssemblyCompiler<TContext> where TContext : IAssemblyCompilerContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Assembly CompileAssembly(TContext context);
	}
}