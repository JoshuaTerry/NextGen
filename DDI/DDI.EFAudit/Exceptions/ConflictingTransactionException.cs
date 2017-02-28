using System;
using System.Data.Entity.Core;

namespace DDI.EFAudit.Exceptions
{
    public class ConflictingTransactionException : Exception
    {
        private const string expectedMessage1 =  "The underlying provider failed on EnlistTransaction.";
        private const string expectedMessage2 =  "Cannot enlist in the transaction because a local transaction is in progress on the connection.  Finish local transaction and retry.";
        private const string message = "Attempted to open a transaction on the current connection, but there is already a transaction in progress. ";

        public ConflictingTransactionException(EntityException e)
            : base(message, e) { }

        /// <summary>
        /// It's difficult to detect exceptions correctly because EF doesn't through proper 
        /// Exception Types.  You have to match on the exact message which might be unreliable.
        /// If it fails default to just a raw Entity Exception. 
        /// </summary>
        public static bool Matches(EntityException e)
        {
            return e.Message == expectedMessage1
                && e.InnerException != null 
                && e.InnerException is InvalidOperationException
                && e.InnerException.Message == expectedMessage2;
        }
    }
}
