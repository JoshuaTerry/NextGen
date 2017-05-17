using DDI.Shared;
using System.Data.Entity;
using System.Linq;
using System;

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
        public void SetNextSequenceValue(string sequenceName, Int64 newValue)
        {
            _context.Database.ExecuteSqlCommand($"ALTER SEQUENCE {sequenceName} RESTART WITH {newValue};");
        }

        /// <summary>
        /// Get the next value for a database sequence.
        /// </summary>
        public Int64 GetNextSequenceValue(string sequenceName)
        {
            return _context.Database.SqlQuery<Int64>($"SELECT NEXT VALUE FOR {sequenceName};").FirstOrDefault();
        }
        
    }
}
