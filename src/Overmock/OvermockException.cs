namespace Overmock
{
    public class OvermockException : Exception
    {
        public OvermockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}