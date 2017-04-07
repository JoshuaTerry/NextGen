using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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
        [Index("IX_Code", IsUnique = true)]
        public string Code { get; set; }         

        [MaxLength(128)]
        [Index("IX_Name", IsUnique = true)]
        public string Name { get; set; }       

        public int NumberOfSegments { get; set; }

        public LedgerStatus Status { get; set; }

        [MaxLength(40)]
        public string FixedBudgetName { get; set; }

        [MaxLength(40)]
        public string WorkingBudgetName { get; set; }

        [MaxLength(40)]
        public string WhatIfBudgetName { get; set; }

        public bool ApproveJournals { get; set; }

        public bool FundAccounting { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        public bool PostAutomatically { get; set; }

        public int PostDaysInAdvance { get; set; }

        public Guid? OrgLedgerId { get; set; }
        [ForeignKey(nameof(OrgLedgerId))]
        public Ledger OrgLedger { get; set; } 

        public PriorPeriodPostingMode PriorPeriodPostingMode { get; set; }

        public bool CapitalizeHeaders { get; set; }       

        public bool CopyCOAChanges { get; set; }       

        public int AccountGroupLevels { get; set; }

        [MaxLength(40)]
        public string AccountGroup1Title { get; set; }

        [MaxLength(40)]
        public string AccountGroup2Title { get; set; }

        [MaxLength(40)]
        public string AccountGroup3Title { get; set; }

        [MaxLength(40)]
        public string AccountGroup4Title { get; set; }

        public ICollection<SegmentLevel> SegmentLevels { get; set; }
        public ICollection<LedgerAccount> LedgerAccounts { get; set; }

        [InverseProperty(nameof(FiscalYear.Ledger))]
        public ICollection<FiscalYear> FiscalYears { get; set; }

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

        
        [NotMapped]
        public string DisplayFormat
        {
            get
            {
                if (SegmentLevels == null)
                {
                    return "";
                }
                else
                {
                    string displayFormat = "";

                    foreach (SegmentLevel sl in SegmentLevels.OrderBy(t => t.Level))
                    {
                        string character = "";
                        switch (sl.Format)
                        {
                            case SegmentFormat.Both:
                                character = "X";
                                break;
                            case SegmentFormat.Numeric:
                                character = "9";
                                break;
                            case SegmentFormat.Alpha:
                                character = "A";
                                break;
                        }

                        for (int k = 1; k <= sl.Length; k++){

                            displayFormat = displayFormat + character;
                        }

                        if (sl.Separator != null)
                        {
                            displayFormat = displayFormat + sl.Separator;
                        }
                                    
                    }

                    return displayFormat;

                }
            }
        }

    }
}
