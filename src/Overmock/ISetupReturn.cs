using System;

namespace Overmocked
{
    /// <summary>
    /// Represents a member that can setup a return value.
    /// </summary>
    /// <typeparam name="TReturn">The type of the t return.</typeparam>
    public interface ISetupReturn<TReturn> : ISetup
    {
        /// <summary>
        /// Sets the value used as the result when calling this overmock's object.
        /// </summary>
        /// <param name="resultProvider">The result provider.</param>
        IOvermock ToReturn(Func<TReturn> resultProvider);

        /// <summary>
        /// Sets the value used as the result when calling this overmock's object.
        /// </summary>
        /// <param name="result">The result.</param>
        IOvermock ToReturn(TReturn result);
    }
}
