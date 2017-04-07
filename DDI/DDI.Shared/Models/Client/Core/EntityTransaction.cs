using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Enums.Core;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityTransaction")]
    public class EntityTransaction : LinkedEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; } 

        public EntityTransactionCategory Category { get; set; }

        public TransactionAmountType AmountType { get; set; }

        public Guid? TransactionId { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }

        
    }
}
