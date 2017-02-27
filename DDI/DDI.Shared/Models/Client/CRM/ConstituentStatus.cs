using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Enums.CRM;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("ConstituentStatus")]
    public class ConstituentStatus : EntityBase, ICodeEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }


        [MaxLength(16)]
        public string Code { get; set; }

        public bool IsActive { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

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
