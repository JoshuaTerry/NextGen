using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.INV
{
    [Table("Investment")]
    public class Investment : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }
        
        public string CUSIP { get; set; }

        [MaxLength(256)]
        public string InvestmentDescription { get; set; }

        //per Pat, need to be able to add to these types
        [ForeignKey("InvestmentOwnershipTypeId")]
        public InvestmentOwnershipType InvestmentOwnershipType { get; set; }
        
        public int InvestmentNumber { get; set; }
        
        public InvestmentStatus InvestmentStatus { get; set; }
        
        public DateTime? InvestmentStatusDate { get; set; }

        [ForeignKey("InvestmentTypeId")]
        public InvestmentType InvestmentType { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CurrentMaturityDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OriginalMaturityDate { get; set; }

        public decimal Rate { get; set; }



        #region Navigation Properties
        #endregion

        #region NotMapped Properties

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return InvestmentNumber + ": " + InvestmentDescription;
            }
        }

        [NotMapped]
        public decimal OriginalPrincipalBalance
        {
            get
            {
                return 0;
            }
        }

        [NotMapped]
        public decimal Balance
        {
            get
            {
                return new decimal(1234.56);
            }
        }

        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
