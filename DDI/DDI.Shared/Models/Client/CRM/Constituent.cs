using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Shared.Attributes;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CP;
using DDI.Shared.Statics;

namespace DDI.Shared.Models.Client.CRM
{
    [Table("Constituent"), Hateoas(RouteNames.Constituent)]
    public class Constituent : EntityBase, IEntity
    {
        #region Public Properties        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override Guid Id { get; set; }

        public DateTime? BirthDate { get; set; }
        [NotMapped]
        public int? AgeRangeFrom { get; set; }
        [NotMapped]
        public int? AgeRangeTo { get; set; }
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

        public PaymentMethod PreferredPaymentMethod { get; set; }

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

        public ClergyStatus ClergyStatus { get; set; }
        public ClergyType ClergyType { get; set; }
        public ConstituentStatus ConstituentStatus { get; set; }
        public ConstituentType ConstituentType { get; set; }
        public EducationLevel EducationLevel { get; set; }
        public Gender Gender { get; set; }
        public IncomeLevel IncomeLevel { get; set; }
        public Language Language { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public Prefix Prefix { get; set; }
        public Profession Profession { get; set; }

        [HateoasCollectionLink(RouteNames.ConstituentAddress)]
        public ICollection<ConstituentAddress> ConstituentAddresses { get; set; }
        [HateoasCollectionLink(RouteNames.AlternateId)]
        public ICollection<AlternateId> AlternateIds { get; set; }
        [HateoasCollectionLink(RouteNames.ContactInfo)]
        public ICollection<ContactInfo> ContactInfo { get; set; }
        [HateoasCollectionLink(RouteNames.Denomination)]
        public ICollection<Denomination> Denominations { get; set; }
        [HateoasCollectionLink(RouteNames.DoingBusinessAs)]
        public ICollection<DoingBusinessAs> DoingBusinessAs { get; set; }
        [HateoasCollectionLink(RouteNames.Education)]
        public ICollection<Education> Educations { get; set; }
        [HateoasCollectionLink(RouteNames.Ethnicity)]
        public ICollection<Ethnicity> Ethnicities { get; set; }
        [HateoasCollectionLink(RouteNames.PaymentPreference)]
        public ICollection<PaymentPreference> PaymentPreferences { get; set; }
        [HateoasCollectionLink(RouteNames.Tag)]
        public ICollection<Tag> Tags { get; set; }

        public ICollection<PaymentMethodBase> PaymentMethods { get; set; }

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
        #endregion

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
