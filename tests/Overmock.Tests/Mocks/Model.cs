namespace Overmock.Tests.Mocks
{
    public class Model
    {
        public int Id { get; set; }

        public string GetName()
        {
            return "Hello, world!";
        }
    }
}