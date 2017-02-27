using System;

namespace DDI.EFAudit.Exceptions
{
    public class UnableToInstantiateObjectException : Exception
    {
        private readonly Type type;

        private const string message =
            "Attempted to instantiate an object of type '{0}', but failed because the object does not have a public or private paramterless constructor.";

        public UnableToInstantiateObjectException(Type type, Exception innerException = null)
            : base(string.Format(message, type), innerException)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }
}
