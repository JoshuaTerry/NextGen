using DDI.Shared.Enums.CRM;
using DDI.Shared.Enums.INV;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Models.Client.INV;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Constituent")]
    public class Constituent : AuditableEntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public BirthDateType BirthDateType { get; set; }

        public int? BirthMonth { get; set; }

        public int? BirthDay { get; set; }

        public int? BirthYearFrom { get; set; }

        public int? BirthYearTo { get; set; }

        [MaxLength(128)]
        public string Business { get; set; }

        public Guid? ClergyStatusId { get; set; }

        public Guid? ClergyTypeId { get; set; }

        public int ConstituentNumber { get; set; }

        public Guid? ConstituentStatusId { get; set; }

        public DateTime? ConstituentStatusDate { get; set; }

        public Guid? ConstituentTypeId { get; set; }

        public CorrespondencePreference CorrespondencePreference { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DeceasedDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DivorceDate { get; set; }

        public Guid? EducationLevelId { get; set; }

        public string Employer { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmploymentEndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EmploymentStartDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? FirstEmploymentDate { get; set; }

        [MaxLength(128)]
        public string FirstName { get; set; }

        [MaxLength(255)]
        public string FormattedName { get; set; }

        public Guid? GenderId { get; set; }

        public Guid? IncomeLevelId { get; set; }

        public bool IsEmployee { get; set; }

        public bool IsIRSLetterReceived { get; set; }

        public bool IsTaxExempt { get; set; }

        public Guid? LanguageId { get; set; }

        [MaxLength(128)]
        public string LastName { get; set; }

        public Guid? MaritalStatusId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? MarriageDate { get; set; }

        public int? MembershipCount { get; set; }

        [MaxLength(128)]
        public string MiddleName { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        [MaxLength(128)]
        public string Name2 { get; set; }

        [MaxLength(128)]
        public string NameFormat { get; set; }

        [MaxLength(128)]
        public string Nickname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? OrdinationDate { get; set; }

        public PaymentMethodType PreferredPaymentMethod { get; set; }

        [MaxLength(128)]
        public string PlaceOfOrdination { get; set; }

        [MaxLength(128)]
        public string Position { get; set; }

        public Guid? PrefixId { get; set; }

        public Guid? ProfessionId { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ProspectDate { get; set; }

        public SalutationType SalutationType { get; set; }

        [MaxLength(255)]
        public string Salutation { get; set; }

        [MaxLength(128)]
        public string Source { get; set; }

        [MaxLength(128)]
        public string Suffix { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TaxExemptVerifyDate { get; set; }

        [MaxLength(128)]
        public string TaxId { get; set; }

        public int? YearEstablished { get; set; }

        #region Navigation Properties
        [ForeignKey("ClergyStatusId")]
        public ClergyStatus ClergyStatus { get; set; }
        [ForeignKey("ClergyTypeId")]
        public ClergyType ClergyType { get; set; }
        [ForeignKey("ConstituentStatusId")]
        public ConstituentStatus ConstituentStatus { get; set; }
        [ForeignKey("ConstituentTypeId")]
        public ConstituentType ConstituentType { get; set; }
        [ForeignKey("EducationLevelId")]
        public EducationLevel EducationLevel { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }
        [ForeignKey("IncomeLevelId")]
        public IncomeLevel IncomeLevel { get; set; }
        [ForeignKey("LanguageId")]
        public Language Language { get; set; }
        [ForeignKey("MaritalStatusId")]
        public MaritalStatus MaritalStatus { get; set; }
        [ForeignKey("PrefixId")]
        public Prefix Prefix { get; set; }
        [ForeignKey("ProfessionId")]
        public Profession Profession { get; set; }



        public ICollection<ConstituentAddress> ConstituentAddresses { get; set; }

        public ICollection<AlternateId> AlternateIds { get; set; }

        public ICollection<ContactInfo> ContactInfo { get; set; }

        public ICollection<Denomination> Denominations { get; set; }

        public ICollection<DoingBusinessAs> DoingBusinessAs { get; set; }

        public ICollection<Education> Educations { get; set; }

        public ICollection<Ethnicity> Ethnicities { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public ICollection<InvestmentRelationship> InvestmentRelationships { get; set; }

        [InverseProperty(nameof(Relationship.Constituent1))]
        public ICollection<Relationship> Relationship1s { get; set; }

        [InverseProperty(nameof(Relationship.Constituent2))]
        public ICollection<Relationship> Relationship2s { get; set; }
        #endregion

        #region NotMapped Properties

        [NotMapped]
        public override string DisplayName
        {
            get
            {
                return FormattedName;
            }
        }

        [NotMapped]
        public int? AgeFrom { get; set; }

        [NotMapped]
        public int? AgeTo { get; set; }

        [NotMapped]
        public int? BirthYear { get; set; }

        [NotMapped]
        public InvestorStatus InvestorStatus
        {
            get
            {
                return InvestorStatus.ActiveInvestor;
            }
        }

        [NotMapped]
        public string InvestorStatusDescription
        {
            get
            {
                return EnumHelper.GetDescription(InvestorStatus);
            }
        }

        [NotMapped]
        public DateTime? InvestorStartDate
        {
            get
            {
                return new DateTime(2000, 01, 05);
            }
        }

        [NotMapped]
        public decimal PrimaryInvestorTotal
        {
            get
            {
                return new decimal(10000.43);
            }
        }

        [NotMapped]
        public decimal JointInvestorTotal
        {
            get
            {
                return new decimal(5042.43);
            }
        }

        [NotMapped]
        public decimal InvestorTotal
        {
            get
            {
                decimal investorTotal = (PrimaryInvestorTotal + JointInvestorTotal);
                return new decimal(15042.86);
            }
        }
        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        #endregion Private Properties
    }
}
