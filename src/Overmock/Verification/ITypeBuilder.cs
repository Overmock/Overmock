namespace Overmock.Verification
{
    public interface ITypeBuilder
    {
        T BuildType<T>(IOvermock<T> target) where T : class;
    }
}