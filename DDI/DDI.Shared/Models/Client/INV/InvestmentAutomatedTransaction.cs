using DDI.Shared.Enums.CRM;
using DDI.Shared.Enums.Core;
using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.CRM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.INV
{
    [Table("InvestmentAutomatedTransaction")]
    public class InvestmentAutomatedTransaction : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        [ForeignKey("InvestmentId")]
        public Investment Investment { get; set; }

        public Guid? InvestmentId { get; set; }

        public InvestmentAutomatedTransactionMethod InvestmentAutomatedTransactionMethod { get; set; }

        public DateTime NextTransactionDate { get; set; }
        
        public RecurringType RecurringType { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey("PaymentMedthodId")]
        public PaymentMethod PaymentMethod { get; set; }

        public Guid? PaymentMethodId { get; set; }

        public bool IsActive { get; set; }

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
