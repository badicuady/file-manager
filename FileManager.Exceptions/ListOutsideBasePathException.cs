using System;
using System.Runtime.Serialization;

namespace FileManager.Exceptions
{
    [Serializable]
    public class ListOutsideBasePathException : Exception
    {
        public ListOutsideBasePathException() : base(ExceptionMessages.ListOutsideBasePathExceptionMessage)
        {
        }

        public ListOutsideBasePathException(string message) : base(message)
        {
        }

        public ListOutsideBasePathException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ListOutsideBasePathException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
