
namespace Overmock.Tests.Mocks
{
    public class Work : IDidWork<string>
    {
        private readonly string _work;

        public Work(string work)
        {
            _work = work;
        }

        public string Result => _work;
    }
}