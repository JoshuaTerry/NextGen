using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;

namespace DDI.Shared.Models.Client.GL
{
    public class Account : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }
        public FiscalYear FiscalYear { get; set; }

        [MaxLength(128)]
        public string AccountNumber { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        // JLT - What happens when you Deactivate an Account?
        public bool IsActive { get; set; } 

        [DecimalPrecision(14, 2)]
        public decimal BeginningBalance { get; set; }
        // JLT - Account has this Category but then AccountGroup also 
        // has a Category and the Account can be associated with 
        // up to 4 of those.  Should all of these Categories be the
        // the same or can they vary between Groups or even Account and
        // Group?
        public AccountCategory Category { get; set; }            
            
        // JLT - Do we still need this?
        //public bool IsNormallyDebit { get; set; } 

        [Index]
        public string SortKey { get; set; }

        public PeriodAmountList Debit { get; set; }

        public PeriodAmountList Credit { get; set; }

             
        public Guid? ClosingAccountId { get; set; }
        public Account ClosingAccount { get; set; }

        public Guid? Group1Id { get; set; }
        public AccountGroup Group1 { get; set; }

        public Guid? Group2Id { get; set; }
        public AccountGroup Group2 { get; set; }

        public Guid? Group3Id { get; set; }
        public AccountGroup Group3 { get; set; }

        public Guid? Group4Id { get; set; }
        public AccountGroup Group4 { get; set; }

        // JLT - In the previous model the relationship went:
        // BankAccount -> LedgerAccounts -> Ledger -> Fiscal Year -> LedgerAccountYear -> Account & SubLedgerTrans
        //
        // With the properties here I'm not sure I understand what relationships are?
        // Shouldn't the LedgerAccounts be available through the LedgerAccountYears object?
        // The LedgerAccountYear object has back to the account as well as a reference
        // to a LedgerAccount.
        // How will these relate to the controller exactly?  It almost seems like the LedgerAccountYear
        // controller, depending on its includes, would do most of the work?
        public ICollection<LedgerAccountYear> LedgerAccountYears { get; set; }
        public ICollection<LedgerAccount> LedgerAccounts { get; set; }
        public ICollection<AccountBudget> Budgets { get; set; }        
        public ICollection<AccountSegment> AccountSegments { get; set; }

        [InverseProperty(nameof(AccountPriorYear.PriorAccount))]
        public ICollection<AccountPriorYear> PriorYearAccounts { get; set; }

        // JLT - Why are the Next Year Accounts of type AccountPriorYear?
        [InverseProperty(nameof(AccountPriorYear.Account))]
        public ICollection<AccountPriorYear> NextYearAccounts { get; set; }

    }
}
