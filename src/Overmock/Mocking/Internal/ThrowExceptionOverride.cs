namespace Overmock.Mocking.Internal
{
    public class ThrowExceptionOverride : IOverride
    {
        internal ThrowExceptionOverride(Exception? exception = default)
        {
            Exception = exception;
        }

        public Exception? Exception { get; }
    }
}