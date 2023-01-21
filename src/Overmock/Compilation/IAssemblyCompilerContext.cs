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
}