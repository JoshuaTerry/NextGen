using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class FiscalPeriod : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? FiscalYearId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public bool IsAdjustmentPeriod { get; set; }

        public FiscalPeriodStatus Status { get; set; }
         
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear {get; set;}

        // JFA: 
        // PeriodNumber is definitely needed.  Periods are numbered 1 thru 14 (which is the max).


    }
}
