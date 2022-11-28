namespace Overmock.Mocking.Internal
{
    public class VerifyException : Exception
    {
        public VerifyException(string message, IVerifiable verifiable, Exception? innerException = default) : base (message, innerException)
        {
            Verifiable = verifiable;
        }

        public IVerifiable Verifiable { get; }
    }
}