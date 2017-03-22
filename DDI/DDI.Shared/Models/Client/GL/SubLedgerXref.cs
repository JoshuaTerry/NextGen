using DDI.Shared.Enums.GL;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.GL
{
    [Table("GL_SubledgerXref")]
    public class SubLedgerXref : EntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; } 

        public TransactionXrefCategory Category { get; set; }

        public Guid? SubledgerTransactionId { get; set; }
        [ForeignKey(nameof(SubledgerTransactionId))]
        public SubledgerTransaction SubledgerTransaction { get; set; }

        // TBD
        // public Guid? DocumentTransactionId { get; set; }
        // public DocumentTransaction DocumentTransaction { get; set; }

    }
}
