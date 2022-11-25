namespace Overmock
{
    public interface IOvermockBuilder
    {
        ITypeBuilder GetTypeBuilder(Action<SetupArgs>? argsProvider = null);
    }
}