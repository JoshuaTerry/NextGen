using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class FiscalYear : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? LedgerId { get; set; }
        [ForeignKey(nameof(LedgerId))]
        public Ledger Ledger { get; set; }

        [MaxLength(255)]
        public string YearName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public FiscalYearStatus Status { get; set; }

        public int NumberOfPeriods { get; set; }

        // JLT Why a byte?
        public byte CurrentPeriodNumber { get; set; }

        public ICollection<FiscalPeriod> FiscalPeriods { get; set; }

        public ICollection<LedgerAccount> LedgerAccounts { get; set; }

        public ICollection<Segment> Segments { get; set; }
        
        // JFA:
        // CurrentPeriod is definitely required.
        // NumberOfPeriods may be helpful to avoid having to drill into FiscalPeriod to count them.
        // JT: How does Current Period and Number of Periods get updated or stay up to date if they are 
        // stored as a field value?
    }
}
