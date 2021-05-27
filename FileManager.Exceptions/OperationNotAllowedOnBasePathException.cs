using System;
using System.Runtime.Serialization;

namespace FileManager.Exceptions
{
    [Serializable]
    public class OperationNotAllowedOnBasePathException : Exception
    {
        public OperationNotAllowedOnBasePathException() : base(ExceptionMessages.OperationNotAllowedOnBasePathExceptionMessage)
        {
        }

        public OperationNotAllowedOnBasePathException(string message) : base(message)
        {
        }

        public OperationNotAllowedOnBasePathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OperationNotAllowedOnBasePathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
