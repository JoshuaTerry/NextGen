using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("Account")]
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

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; } 

        [DecimalPrecision(14, 2)]
        public decimal BeginningBalance { get; set; }

        public AccountCategory Category { get; set; }            
            
        public bool IsNormallyDebit { get; set; } 

        [Index,MaxLength(128)]
        public string SortKey { get; set; }      
             
        public Guid? ClosingAccountId { get; set; }
        [ForeignKey(nameof(ClosingAccountId))]
        public Account ClosingAccount { get; set; }

        public Guid? Group1Id { get; set; }
        [ForeignKey(nameof(Group1Id))]
        public AccountGroup Group1 { get; set; }

        public Guid? Group2Id { get; set; }
        [ForeignKey(nameof(Group2Id))]
        public AccountGroup Group2 { get; set; }

        public Guid? Group3Id { get; set; }
        [ForeignKey(nameof(Group3Id))]
        public AccountGroup Group3 { get; set; }

        public Guid? Group4Id { get; set; }
        [ForeignKey(nameof(Group4Id))]
        public AccountGroup Group4 { get; set; }

        public ICollection<LedgerAccountYear> LedgerAccountYears { get; set; }
        public ICollection<AccountBudget> Budgets { get; set; }        
        public ICollection<AccountSegment> AccountSegments { get; set; }

        [InverseProperty(nameof(AccountPriorYear.PriorAccount))]
        public ICollection<AccountPriorYear> PriorYearAccounts { get; set; }

        [InverseProperty(nameof(AccountPriorYear.Account))]
        public ICollection<AccountPriorYear> NextYearAccounts { get; set; }

        [InverseProperty(nameof(AccountBalance.Account))]
        public ICollection<AccountBalance> AccountBalances { get; set; }

        public override string DisplayName => AccountNumber;
    }
}
