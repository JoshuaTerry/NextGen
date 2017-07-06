using DDI.Shared.Enums.Core;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityTransaction")]
    public class EntityTransaction : EntityBase, ILinkedEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index]
        public Guid? ParentEntityId { get; set; }

        [Index]
        [MaxLength(128)]
        public string EntityType { get; set; }

        public EntityTransactionRelationship Relationship { get; set; }

        public EntityTransactionCategory Category { get; set; }

        public TransactionAmountType AmountType { get; set; }

        public Guid? TransactionId { get; set; }
        [ForeignKey(nameof(TransactionId))]
        public Transaction Transaction { get; set; }

        
    }
}
