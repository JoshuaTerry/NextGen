using System;

namespace DDI.Shared
{
    /// <summary>
    /// Exception thrown by validation in business logic.
    /// </summary>
    public class ValidationException : ApplicationException
    {
        public ValidationException() : base() { }
        public ValidationException(string message) : base(message) { }
        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
