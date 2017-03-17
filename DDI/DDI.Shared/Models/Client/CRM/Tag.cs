using DDI.Shared.Enums.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Tag")]
    public class Tag : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [MaxLength(16)]
        public string Code { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public Guid? TagGroupId { get; set; }
        [ForeignKey("TagGroupId")]
        public TagGroup TagGroup { get; set; }

        public int Order { get; set; }

        public ConstituentCategory ConstituentCategory { get; set; }

        public bool IsActive { get; set; }

        public ICollection<Constituent> Constituents { get; set; }

        public ICollection<ConstituentType> ConstituentTypes { get; set; }

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
