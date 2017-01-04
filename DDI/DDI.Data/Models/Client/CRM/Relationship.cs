using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client.CRM
{
    [Table("Relationsihp")]
    public class Relationship : BaseEntity
    {
        #region Public Properties   
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public Guid? RelationshipTypeId { get; set; }

        public RelationshipType RelationshipType { get; set; }

        public Guid? Constituent1Id { get; set; }

        public Constituent Constituent1 { get; set; }

        public Guid? Constituent2Id { get; set; }

        public Constituent Constituent2 { get; set; }

        #endregion Public Properties

        #region Public Methods

        #endregion

    }
}
