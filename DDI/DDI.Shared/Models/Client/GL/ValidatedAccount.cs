using System.Collections.Generic;

namespace DDI.Shared.Models.Client.GL
{
    /// <summary>
    /// A non-database model for an account number that has been validated.
    /// </summary>
    public class ValidatedAccount
    {
        public BusinessUnit ExplicitBusinessUnit { get; set; }
        public Account Account { get; set; }
        public LedgerAccount LedgerAccount { get; set; }
        public List<Segment> Segments { get; set; }
        public List<string> SegmentCodes { get; set; }
        public string AccountNumber { get; set; }

        public ValidatedAccount()
        {
            Segments = new List<Segment>();
            SegmentCodes = new List<string>();
            AccountNumber = string.Empty;
        }
    }
}
