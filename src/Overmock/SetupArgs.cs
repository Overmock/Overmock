namespace Overmock
{
    /// <summary>
    /// Used to pass constructor parameters when creating mocked objects.
    /// </summary>
    public class SetupArgs
    {
        private object?[] _args = Array.Empty<object>();

        /// <summary>
        /// Sets the arguments to use.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Args(params object?[] args)
        {
            _args = args;
        }

        internal object?[] Parameters => _args;
    }
}