using System.Reflection;

namespace Overmock.Compilation
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TContext"></typeparam>
	public abstract class AssemblyCompiler<TContext> : IAssemblyCompiler<TContext> where TContext : IAssemblyCompilerContext
	{
        /// <summary>
		/// 
		/// </summary>
		protected AssemblyCompiler(IOvermockMethodBuilder methodBuilder)
        {
            MethodBuilder = methodBuilder;
        }

        /// <summary>
        /// 
        /// </summary>
        public IOvermockMethodBuilder MethodBuilder { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns>The compiled <see cref="Assembly" />.</returns>
        public abstract Assembly CompileAssembly(TContext context);
	}
}