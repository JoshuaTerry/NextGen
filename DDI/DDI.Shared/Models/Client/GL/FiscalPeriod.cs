using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("FiscalPeriod")]
    public class FiscalPeriod : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index("IX_FiscalYear_PeriodNumber", IsUnique = true, Order = 1)]
        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        [Index("IX_FiscalYear_PeriodNumber", IsUnique = true, Order = 2)]
        public int PeriodNumber { get; set; }

        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }

        public bool IsAdjustmentPeriod { get; set; }

        public FiscalPeriodStatus Status { get; set; }

        public override string DisplayName
        {
            get
            {
                if (FiscalYear != null)
                {
                    return $"{FiscalYear.DisplayName}:{PeriodNumber}";
                }
                return PeriodNumber.ToString();
            }
        }

    }
}
