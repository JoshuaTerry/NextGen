using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class Account : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }
        [MaxLength(128)]
        public string AccountNumber { get; set; }
        [MaxLength(255)]
        public string Description { get; set; }
        public bool IsActive { get; set; } 

        // Made these non-null.  The Decimal Precision has to be set in the DBContext in 
        // the OnModelCreating method.
        public decimal BeginningBalance { get; set; }
        public decimal EndingBalance { get; set; }
        public AccountCategory Category { get; set; }                
        // Made this a collection
        public ICollection<LedgerAccount> LedgerAccounts { get; set; } 
        public ICollection<Budget> Budgets { get; set; }
        public ICollection<LedgerAccountYear> LedgerAccountYears { get; set; }
        public bool IsNormallyDebit { get; set; } // Added
        public string SortKey { get; set; } // Added
 

        // Definitely need the actuals.  There are fourteen for debits, and fourteen for credits.
         
        // Also needed: 
        //  AccountSegments collection
        //  NextYearAccounts collection (see note below)
        //  PriorYearAccounts collection (see note below)
        //  ClosingAccount (Account)
        //  Group1 thru Group4 (AccountGroup)

        // AccountPriorYear entity is missing and needs to be added.

    }
}
