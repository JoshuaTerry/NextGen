using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Attributes.Models;

namespace DDI.Shared.Models.Client.GL
{
    [Table("LedgerAccount"),EntityName("G/L Account")]
    public class LedgerAccount : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? LedgerId { get; set; }
        [ForeignKey(nameof(LedgerId))]
        public Ledger Ledger { get; set; }

        [MaxLength(128)]
        public string AccountNumber { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public ICollection<LedgerAccountYear> LedgerAccountYears { get; set; }

        public override string DisplayName => AccountNumber;
    }
}
