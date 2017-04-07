using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.INV
{
    [Table("InvestmentType")]
    public class InvestmentType : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }


        public int Type { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        //more fields to be added later
        #region Navigation Properties
        #endregion

        #region NotMapped Properties

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return Type + ": " + Description;
            }
        }

        
        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
