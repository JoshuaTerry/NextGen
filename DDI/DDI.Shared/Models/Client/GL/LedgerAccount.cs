using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class LedgerAccount : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? LedgerId { get; set; }
        public Ledger Ledger { get; set; }

        [MaxLength(128)]
        public string AccountNumber { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public ICollection<LedgerAccountYear> LedgerAccountYears { get; set; } 
    }
}
