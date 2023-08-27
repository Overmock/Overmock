using System;

namespace Overmocked
{
    /// <summary>
    /// Used to pass constructor parameters when creating mocked objects.
    /// </summary>
    public class SetupArgs
    {
        /// <summary>
        /// The arguments
        /// </summary>
        private object?[] _args = Array.Empty<object>();

        /// <summary>
        /// Sets the arguments to use.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Args(params object?[] args)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public object?[] Parameters => _args;
    }
}