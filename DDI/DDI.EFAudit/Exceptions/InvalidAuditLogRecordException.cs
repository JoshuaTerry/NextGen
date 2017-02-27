using System;

namespace DDI.EFAudit.Exceptions
{
    public class InvalidAuditLogRecordException : Exception
    {
        private readonly object record;

        public InvalidAuditLogRecordException(string message, object record)
            : base(string.Format(message, record))
        {
            this.record = record;
        }

        public object Record
        {
            get { return record; }
        }
    }
}
