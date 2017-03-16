using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq; 
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Relationship")]
    public class Relationship : AuditableEntityBase
    {
        #region Public Properties   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? RelationshipTypeId { get; set; }
        [ForeignKey("RelationshipTypeId")]
        public RelationshipType RelationshipType { get; set; }

        public Guid? Constituent1Id { get; set; }
        [ForeignKey("Constituent1Id")]
        public Constituent Constituent1 { get; set; }

        public Guid? Constituent2Id { get; set; }
        [ForeignKey("Constituent2Id")]
        public Constituent Constituent2 { get; set; }

        [NotMapped]
        public bool IsSwapped { get; set; }

        [NotMapped]
        public Guid? TargetConstituentId { get; set; }

        public override string DisplayName => $"{Constituent1?.ConstituentNumber} is the {RelationshipType?.Name} of {Constituent2?.ConstituentNumber}";

        #endregion Public Properties

        #region Public Methods

        #endregion

    }
}
