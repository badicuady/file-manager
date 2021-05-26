using System;
using System.Runtime.Serialization;

namespace FileManager.Exceptions
{
    [Serializable]
    public class DirectoryDoesNotExistsException : Exception
    {
        public DirectoryDoesNotExistsException() : base(ExceptionMessages.DirectoryDoesNotExistsExceptionMessage)
        {
        }

        public DirectoryDoesNotExistsException(string message) : base(message)
        {
        }

        public DirectoryDoesNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DirectoryDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
