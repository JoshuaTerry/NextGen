using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("RelationshipCategory")]
    public class RelationshipCategory : AuditableEntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(4)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public bool IsShownInQuickView { get; set; }

        public ICollection<RelationshipType> RelationshipTypes { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Code + ": " + Name;
            }
        }

        #endregion

    }
}
