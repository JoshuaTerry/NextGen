using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("FiscalYear")]
    public class FiscalYear : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_LedgerName", Order = 1, IsUnique = true)]
        public Guid? LedgerId { get; set; }
        [ForeignKey(nameof(LedgerId))]
        public Ledger Ledger { get; set; }

        [Index("IX_LedgerName", Order = 2, IsUnique = true), MaxLength(16)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public FiscalYearStatus Status { get; set; }

        public int NumberOfPeriods { get; set; }
         
        public int CurrentPeriodNumber { get; set; }

        public ICollection<FiscalPeriod> FiscalPeriods { get; set; }

        public ICollection<LedgerAccountYear> LedgerAccounts { get; set; }

        public ICollection<Segment> Segments { get; set; }
         
    }
}
