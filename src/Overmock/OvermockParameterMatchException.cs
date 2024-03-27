using System;
using System.Runtime.Serialization;

namespace Overmocked
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class OvermockParameterMatchException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public OvermockParameterMatchException(object expected, object actual) : this($"Parameter '{actual}' does not match expected value. Expected: {expected}, Actual: {actual}")
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public OvermockParameterMatchException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public OvermockParameterMatchException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected OvermockParameterMatchException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}