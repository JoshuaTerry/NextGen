using DDI.Shared.Enums.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityNumber")]
    public class EntityNumber : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public EntityNumberType EntityNumberType { get; set; }

        public Guid? RangeId { get; set; }

        public int NextNumber { get; set; }

        public int PreviousNumber { get; set; }

    }
}
