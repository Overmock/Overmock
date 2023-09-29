using System;
using System.Runtime.Serialization;

namespace Kimono
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class KimonoException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public KimonoException()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public KimonoException(string? message) : base(message)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public KimonoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected KimonoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}