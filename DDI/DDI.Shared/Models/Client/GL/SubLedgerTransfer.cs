using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    public class SubLedgerTransfer : AuditableEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; } 
        public TransactionTransferCategory Category { get; set; }

        public Guid? SubledgerTransactionId { get; set; }
        [ForeignKey(nameof(SubledgerTransactionId))]
        public SubledgerTransaction SubledgerTransaction { get; set; }
          
    }
}
