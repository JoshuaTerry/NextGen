using DDI.Shared.Attributes.Models;
using DDI.Shared.Enums.GL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class AccountPriorYear : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid AccountId { get; set; }
        [ForeignKey(nameof(AccountId))]
        public Account Account { get; set; }

        public Guid PriorAccountId { get; set; }
        [ForeignKey(nameof(PriorAccountId))]
        public Account PriorAccount { get; set; }

        [DecimalPrecision(5, 2)]
        public decimal Percentage { get; set; }

    }
}
