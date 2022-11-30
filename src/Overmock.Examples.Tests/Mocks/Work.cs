
namespace Overmock.Tests.Mocks
{
    public class Worker
    {
        public static IDidWork<T> Work<T>(T result)
        {
            return new Work<T>(result);
        }

        public static WrittenWork Write(string text)
        {
            return new WrittenWork(text);
        }
    }

    public class Work<T> : IDidWork<T>
    {
        public Work(T result)
        {
            Result = result;
        }

        public T Result { get; protected set; }
    }

    public class WrittenWork : Work<string>
    {
        public WrittenWork(string work) : base(work)
        {
        }
    }
}