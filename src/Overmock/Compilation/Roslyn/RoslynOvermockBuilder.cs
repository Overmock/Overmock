namespace Overmock.Compilation.Roslyn
{
    /// <summary>
    /// 
    /// </summary>
    public class RoslynOvermockBuilder : IOvermockBuilder
    {
        ITypeBuilder IOvermockBuilder.GetTypeBuilder(Action<SetupArgs>? argsProvider)
        {
            return new RoslynTypeBuilder(new RoslynAssemblyGenerator(), argsProvider);
        }
    }
}