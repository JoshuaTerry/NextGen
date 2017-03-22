using DDI.Shared.Enums.CRM;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ConstituentStatus")]
    public class ConstituentStatus : AuditableEntityBase, ICodeEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }


        [Index("IX_Code", IsUnique = true), MaxLength(16)]
        public string Code { get; set; }

        [Index("IX_Name", IsUnique = true), MaxLength(128)]
        public string Name { get; set; }

        public bool IsActive { get; set; }

        

        public ConstituentBaseStatus BaseStatus { get; set; }

        public bool IsRequired { get; set; }

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
