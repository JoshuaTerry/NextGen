using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Relationsihp"), Hateoas(RouteNames.Relationship)]
    public class Relationship : EntityBase
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
        public override string DisplayName => $"{Constituent1?.FormattedName} is the {RelationshipType?.Name} of {Constituent2?.FormattedName}";

        #endregion Public Properties

        #region Public Methods

        #endregion

    }
}
