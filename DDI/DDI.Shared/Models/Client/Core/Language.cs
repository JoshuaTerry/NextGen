using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DDI.Shared.Models.Client;

namespace DDI.Shared.Models.Client.Core
{
    [Table("Language")]
    public class Language : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Code { get; set; }

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
