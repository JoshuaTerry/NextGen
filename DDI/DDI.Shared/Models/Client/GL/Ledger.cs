using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class Ledger : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        public Guid? DefaultFiscalYearId { get; set; }
        [ForeignKey(nameof(DefaultFiscalYearId))]
        public FiscalYear DefaultFiscalYear { get; set; }
        public bool IsParent { get; set; }         
        public string Code { get; set; }         
        public string Description { get; set; }
        // JLT Why a byte, is this number of account segments?  
        // How does it get updated or set?
        public byte NumberOfSegments { get; set; }
        public LedgerStatus Status { get; set; }
        [MaxLength(255)]
        public string FixedBudgetName { get; set; }
        [MaxLength(255)]
        public string WorkingBudgetName { get; set; }
        [MaxLength(255)]
        public string WhatIfBudgetName { get; set; }
        public bool ApproveJournals { get; set; }
        public bool FundAccounting { get; set; }
       
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }
        public Guid BusinessUnitId { get; set; }
        public bool PostAutomatically { get; set; }
        public Guid? OrgLedgerId { get; set; }
        [ForeignKey(nameof(OrgLedgerId))]
        public Ledger OrgLedger { get; set; } 
        public bool PostToAllocAccts { get; set; }
        public PriorPeriodPostingMode PriorPeriodPostingMode { get; set; }
        //JLT Does this change on a per ledger basis or is this something that would be set in settings at the business Unit or Org Level
        public bool CapitalizeHeaders { get; set; }
        //JLT Whats this?
        public bool CopyCOAChanges { get; set; }
        //JLT why byte?  What is this?
        public byte RecurringJournalDays { get; set; }
        public int LegacyKey { get; set; }

        //JLT Why a byte?  Shouldn't this be a collection?
        public byte AccountGroupLevels { get; set; } 
        // JLT how do these stay up to date?
        public string AccountGroup1Title { get; set; }         
        public string AccountGroup2Title { get; set; }         
        public string AccountGroup3Title { get; set; }         
        public string AccountGroup4Title { get; set; }

        public ICollection<SegmentLevel> SegmentLevels { get; set; }
        public ICollection<LedgerAccount> LedgerAccounts { get; set; }
        public ICollection<FiscalYear> FiscalYears { get; set; }
        // JFA: 

        // OrgLedger probably needs to be added, as it's a link to the "template" ledger for the organization. All common business units must have ledgers that have common settings.
        // Most of these settings need to be added as well:
        //   AccountGroup1Title - AccountGroup4Title, 
        // JT For the AccountGroupTitle 1 - 4, why do we use strings for these instead of a collection of Account Groups?
        //CapitalizeHeaders, NumSegments (number of segments),  PriorPeriodPostingMode.
        // IsParent may be required for F9.

    }
}
