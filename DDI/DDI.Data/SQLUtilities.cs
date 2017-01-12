using DDI.Shared; 
using System.Data.Entity;
using System.Linq; 

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
        public void SetNextSequenceValue(string sequenceName, int newValue)
        {
            _context.Database.ExecuteSqlCommand($"ALTER SEQUENCE {sequenceName} RESTART WITH {newValue};");
        }

        /// <summary>
        /// Get the next value for a database sequence.
        /// </summary>
        public int GetNextSequenceValue(string sequenceName)
        {
            return _context.Database.SqlQuery<int>($"SELECT NEXT VALUE FOR {sequenceName};").FirstOrDefault();
        }
        
    }
}
