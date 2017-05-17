using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Enums.Core;
using DDI.Shared.Models.Client.GL;
using DDI.Shared.Models.Client.Security;

namespace DDI.Shared.Models.Client.Core
{
    [Table("EntityApproval")]
    public class EntityApproval : EntityBase, ILinkedEntityBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [Index]
        public Guid? ParentEntityId { get; set; }

        [Index]
        [MaxLength(128)]
        public string EntityType { get; set; }

        public Guid? AppprovedById { get; set; }
        [ForeignKey(nameof(AppprovedById))]
        public User ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

    }
}
