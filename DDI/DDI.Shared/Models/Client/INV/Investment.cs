using DDI.Shared.Enums.INV;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.GL;
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

        [ForeignKey("BusinessUnitId")]
        public BusinessUnit BusinessUnit { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public string CUSIP { get; set; }

        [MaxLength(256)]
        public string InvestmentDescription { get; set; }

        //per Pat, need to be able to add to these types
        [ForeignKey("InvestmentOwnershipTypeId")]
        public InvestmentOwnershipType InvestmentOwnershipType { get; set; }

        public Guid? InvestmentOwnershipTypeId { get; set; }

        public int InvestmentNumber { get; set; }

        public InvestmentStatus InvestmentStatus { get; set; }

        public DateTime? InvestmentStatusDate { get; set; }

        [ForeignKey("InvestmentTypeId")]
        public InvestmentType InvestmentType { get; set; }

        public Guid? InvestmentTypeId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? CurrentMaturityDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OriginalMaturityDate { get; set; }

        public MaturityMethod MaturityMethod { get; set; }

        public InvestmentType RenewalInvestmentType { get; set; }

        public DateTime? LastMaturityDate { get; set; }

        public DateTime? MaturityResponseDate { get; set; }

        public int NumberOfRenewals { get; set; }

        [Column(TypeName = "date")]
        public DateTime? PurchaseDate { get; set; }

        public IssuanceMethod IssuanceMethod { get; set; }

        public decimal OriginalPurchaseAmount { get; set; }

        public decimal Rate { get; set; }

        public bool StepUpEligible { get; set; }

        public DateTime? StepUpDate { get; set; }

        public InterestFrequency InterestFrequency { get; set; }

        

        #region Navigation Properties

        public ICollection<InvestmentRelationship> InvestmentRelationship { get; set; }

        public ICollection<InterestPayout> InterestPayout { get; set; }

        #endregion

        #region NotMapped Properties

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return "Testing";
                //return InvestmentNumber + ": " + InvestmentDescription;
            }
        }

        [NotMapped]
        public decimal OriginalPrincipalBalance
        {
            get
            {
                return new decimal(45.54);
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

        [NotMapped]
        public DateTime LastMaintenanceDate
        {
            get
            {
                return new DateTime(2004, 03, 14);
            }
        }

        [NotMapped]
        public DateTime LastTransactionDate
        {
            get
            {
                return new DateTime(2016, 12, 31);
            }
        }

        [NotMapped]
        public decimal AccruedInterest
        {
            get
            {
                return new decimal(34.32);
            }
        }

        [NotMapped]
        public DateTime LastInterestCalculatedDate
        {
            get
            {
                return new DateTime(2017, 05, 02);
            }
        }

        [NotMapped]
        public decimal InterestPaidYTD
        {
            get
            {
                return new decimal(234.43);
            }
        }

        [NotMapped]
        public decimal InterestWithheldYTD
        {
            get
            {
                return new decimal(232.43);
            }
        }

        [NotMapped]
        public decimal PenaltyChargedYTD
        {
            get
            {
                return new decimal(2.43);
            }
        }
        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
