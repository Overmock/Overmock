namespace Overmocked.Benchmarks
{
    public interface IInterceptorMethodCalls
    {
        void Castle(int count);
        void Dotnet(int count);
        void Kimono(int count);
    }
}
