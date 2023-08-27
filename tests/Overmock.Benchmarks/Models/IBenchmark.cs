namespace Overmocked.Benchmarks.Models
{
    public interface IBenchmark
    {
        void VoidNoParams();
        bool BoolNoParams();
        object ObjectNoParams();
        void VoidWith3Params(string name, int age, List<string> list);
    }
}
