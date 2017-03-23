using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("Ledger")]
    public class Ledger : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? DefaultFiscalYearId { get; set; }
        [ForeignKey(nameof(DefaultFiscalYearId))]
        public FiscalYear DefaultFiscalYear { get; set; }

        public bool IsParent { get; set; }         

        [MaxLength(16)]
        public string Code { get; set; }         

        [MaxLength(255)]
        public string Description { get; set; }       

        public int NumberOfSegments { get; set; }

        public LedgerStatus Status { get; set; }

        [MaxLength(255)]
        public string FixedBudgetName { get; set; }

        [MaxLength(255)]
        public string WorkingBudgetName { get; set; }

        [MaxLength(255)]
        public string WhatIfBudgetName { get; set; }

        public bool ApproveJournals { get; set; }

        public bool FundAccounting { get; set; }

        public Guid BusinessUnitId { get; set; }
        public BusinessUnit BusinessUnit { get; set; }

        public bool PostAutomatically { get; set; }

        public Guid? OrgLedgerId { get; set; }
        [ForeignKey(nameof(OrgLedgerId))]
        public Ledger OrgLedger { get; set; } 

        public PriorPeriodPostingMode PriorPeriodPostingMode { get; set; }

        public bool CapitalizeHeaders { get; set; }       

        public bool CopyCOAChanges { get; set; }       

        public int AccountGroupLevels { get; set; }

        [MaxLength(255)]
        public string AccountGroup1Title { get; set; }

        [MaxLength(255)]
        public string AccountGroup2Title { get; set; }

        [MaxLength(255)]
        public string AccountGroup3Title { get; set; }

        [MaxLength(255)]
        public string AccountGroup4Title { get; set; }

        public ICollection<SegmentLevel> SegmentLevels { get; set; }
        public ICollection<LedgerAccount> LedgerAccounts { get; set; }
        public ICollection<FiscalYear> FiscalYears { get; set; }
       
    }
}
