using System;

namespace DDI.Shared.Models.Client.GL
{
    public class FiscalYearTemplate
    {
        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public bool CopyInactiveAccounts { get; set; }
    }
}
