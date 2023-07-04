namespace Overmock.Compilation
{
	/// <summary>
	/// 
	/// </summary>
	public interface IAssemblyCompilerContext : IFluentInterface
	{
		/// <summary>
		/// 
		/// </summary>
		IOvermock Target { get; }


	}

    /// <inheritdoc />
    public abstract class AssemblyCompilerContextBase : IAssemblyCompilerContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        protected AssemblyCompilerContextBase(IOvermock target)
        {
            Target = target;
        }

        public IOvermock Target { get; set; }
    }
}