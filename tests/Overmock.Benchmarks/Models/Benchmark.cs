namespace Overmocked.Benchmarks.Models
{
    public class Benchmark : IBenchmark
    {
        public bool BoolNoParams()
        {
            return true;
        }

        public object ObjectNoParams()
        {
            return new object();
        }

        public void VoidNoParams()
        {
        }

        public void VoidWith3Params(string name, int age, List<string> list)
        {
        }
    }
}
