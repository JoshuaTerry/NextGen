using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models.Client.GL;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityNumber")]
    public class EntityNumber : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public EntityNumberType EntityNumberType { get; set; }

        public Guid? BusinessUnitId { get; set; }
        [ForeignKey(nameof(BusinessUnitId))]
        public BusinessUnit BusinessUnit { get; set; }

        public Guid? FiscalYearId { get; set; }
        [ForeignKey(nameof(FiscalYearId))]
        public FiscalYear FiscalYear { get; set; }

        public int NextNumber { get; set; }

        public int PreviousNumber { get; set; }

    }
}
