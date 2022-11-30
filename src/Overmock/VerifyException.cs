namespace Overmock
{
    public class VerifyException : OvermockException
    {
        public VerifyException(IVerifiable verifiable, string message = "", Exception? innerException = default)
            : base(Ex.Message.General(verifiable, message, innerException), innerException)
        {
            Verifiable = verifiable;
        }

        public IVerifiable Verifiable { get; }
    }
}