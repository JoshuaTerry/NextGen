using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//this may be a more global concept. this definitely needs discussion. 
//this model is here to help provide fake data to the investment detail 
//screen before investments are fully fleshed out
namespace DDI.Shared.Models.Client.INV
{
    [Table("LinkedAccount")]
    public class LinkedAccount : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }

        public Guid? InvestmentId { get; set; }

        public LinkedAccountType LinkedAccountType { get; set; }

        public int LinkedAccountNumber { get; set; }
        
        public CollateralType CollateralType { get; set; }

        public decimal Collateral { get; set; }

        public bool BlockOtherLoanLinks { get; set; }

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
