using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using DDI.Data.Enums;

namespace DDI.Data.Models.Client
{
    [Table("Constituent")]
    public class Constituent
    {
        #region Public Properties

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public BirthDateType BirthDateType { get; set; }

        public int? BirthMonth { get; set; }

        public int? BirthDay { get; set; }

        public int? BirthYearFrom { get; set; }

        public int? BirthYearTo { get; set; }

        [MaxLength(128)]
        public string Business { get; set; }

        public Guid? ClergyStatusId { get; set; }

        public Guid? ClergyTypeId { get; set; }

        public int ConstituentNum { get; set; }

        public Guid? ConstituentStatusId { get; set; }

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

        public int? MaritalStatus { get; set; }

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

        // Navigation Properties

        public virtual ClergyStatus ClergyStatus { get; set; }
        public virtual ClergyType ClergyType { get; set; }
        public virtual ConstituentStatus ConstituentStatus { get; set; }
        public virtual ConstituentType ConstituentType { get; set; }
        public virtual EducationLevel EducationLevel { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual IncomeLevel IncomeLevel { get; set; }
        public virtual Language Language { get; set; }
        public virtual Prefix Prefix { get; set; }
        public virtual Profession Profession { get; set; }

        public virtual ICollection<ConstituentAddress> ConstituentAddresses { get; set; }
        public virtual ICollection<AlternateId> AlternateIds { get; set; }
        public virtual ICollection<ContactInfo> ContactInfo { get; set; }
        public virtual ICollection<Denomination> Denominations { get; set; }
        public virtual ICollection<DoingBusinessAs> DoingBusinessAs { get; set; }
        public virtual ICollection<Education> Educations { get; set; }
        public virtual ICollection<Ethnicity> Ethnicities { get; set; }
        public virtual ICollection<PaymentPreference> PaymentPreferences { get; set; }

        #endregion Public Properties

        #region Private Properties

        //private IUser UserId { get; set; }

        #endregion Private Properties
    }
}
