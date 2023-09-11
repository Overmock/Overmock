using System;
using System.Runtime.Serialization;

namespace Kimono.Core
{
    [Serializable]
    public class KimonoException : Exception
    {
        public KimonoException()
        {
        }

        public KimonoException(string? message) : base(message)
        {
        }

        public KimonoException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected KimonoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}