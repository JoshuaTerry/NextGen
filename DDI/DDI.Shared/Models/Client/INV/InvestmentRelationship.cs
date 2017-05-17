using DDI.Shared.Enums.INV;
using DDI.Shared.Enums;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.INV
{
    [Table("InvestmentRelationship")]
    public class InvestmentRelationship : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [ForeignKey("ConstituentId")]
        public Constituent Constituent { get; set; }

        public Guid? ConstituentId { get; set; }


        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }

        public Guid? InvestmentId { get; set; }

        public InvestmentRelationshipType InvestmentRelationshipType { get; set; }



        #endregion
        #region Navigation Properties

        #region NotMapped Properties

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return "Testing";
                //return InvestmentRelationshipType.ToString() + " for " + Investment.InvestmentNumber + ": " + Investment.InvestmentDescription;
            }
        }

        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
