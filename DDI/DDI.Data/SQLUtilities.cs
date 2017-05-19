using DDI.Shared;
using System.Data.Entity;
using System.Linq;
using System;
using DDI.Shared.Enums.Core;
using DDI.Data.Statics;

namespace DDI.Data
{
    /// <summary>
    /// SQL Utilities
    /// </summary>
    public class SQLUtilities : ISQLUtilities
    {
        private DbContext _context = null;

        public SQLUtilities(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Set the next value for a database sequence.
        /// </summary>
        public void SetNextSequenceValue(DatabaseSequence sequence, Int64 newValue)
        {
            _context.Database.ExecuteSqlCommand($"ALTER SEQUENCE {GetSequenceName(sequence)} RESTART WITH {newValue};");
        }

        /// <summary>
        /// Get the next value for a database sequence.
        /// </summary>
        public Int64 GetNextSequenceValue(DatabaseSequence sequence)
        {
            return _context.Database.SqlQuery<Int64>($"SELECT NEXT VALUE FOR {GetSequenceName(sequence)};").FirstOrDefault();
        }
        
        private string GetSequenceName(DatabaseSequence sequence)
        {
            switch(sequence)
            {
                case DatabaseSequence.TransactionNumber: return Sequences.TransactionNumber;
            }

            throw new ArgumentException("Invalid database sequence", nameof(sequence));
        }
    }
}
