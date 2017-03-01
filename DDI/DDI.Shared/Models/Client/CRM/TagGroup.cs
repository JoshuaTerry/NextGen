using DDI.Shared.Enums.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("TagGroup")]
    public class TagGroup : EntityBase
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public int Order { get; set; }   

        [MaxLength(128)]
        public string Name { get; set; }

        public TagSelectionType TagSelectionType { get; set; }

        public bool IsActive { get; set; }
        
        public ICollection<Tag> Tags { get; set; }

        #endregion Public Properties

        #region Public Methods

        public override string DisplayName
        {
            get
            {
                return Name;
            }
        }

        #endregion

    }
}
