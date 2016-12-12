using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DDI.Data.Models.Client
{
	[Table("Constituent")]
	public class Constituent
	{
        #region Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
		public virtual ICollection<AlternateId> AlternateIds { get; set; }
		public DateTime? BirthDate { get; set; }
		public int? BirthYearFrom { get; set; }
		public int? BirthYearTo { get; set; }
		public string Business { get; set; }
		public ClergyStatus ClergyStatus { get; set; }
		public Guid? ClergyStatusId { get; set; }
		public ClergyType ClergyType { get; set; }
		public Guid? ClergyTypeId { get; set; }
		public int ConstituentNum { get; set; }
		public ConstituentStatus ConstituentStatus { get; set; }
		public Guid? ConstituentStatusId { get; set; }
		public ConstituentType ConstituentType { get; set; }
		public Guid? ConstituentTypeId { get; set; }
		public virtual ICollection<ContactInfo> ContactInfo { get; set; }
		public DateTime? DeceasedDate { get; set; }
		public virtual ICollection<Denomination> Denominations { get; set; }
		public DateTime? DivorceDate { get; set; }
		public virtual ICollection<DoingBusinessAs> DoingBusinessAs { get; set; }
		public EducationLevel EducationLevel { get; set; }
		public virtual ICollection<Education> Educations { get; set; }
		public string Employer { get; set; }
		public DateTime? EmploymentEndDate { get; set; }
		public DateTime? EmploymentStartDate { get; set; }
		public virtual ICollection<Ethnicity> Ethnicities { get; set; }
		public DateTime? FirstEmploymentDate { get; set; }
		public string FirstName { get; set; }
		public string FormattedName { get; set; }
		public Gender Gender { get; set; }
		public Guid? GenderId { get; set; }		
		public IncomeLevel IncomeLevel { get; set; }
		public Guid? IncomeLevelId { get; set; }
		public bool IsEmployee { get; set; }
		public bool IsIRSLetterReceived { get; set; }
		public bool IsTaxExempt { get; set; }
		public Language Language { get; set; }
		public Guid? LanguageId { get; set; }
		public string LastName { get; set; }
		public int? MaritalStatus { get; set; }
		public DateTime? MarriageDate { get; set; }
		public int? MembershipCount { get; set; }
		public string MiddleName { get; set; }
		public string Name2 { get; set; }
		public string Nickname { get; set; }
		[Column(TypeName = "date")]
		public DateTime? OrdinationDate { get; set; }
		public virtual ICollection<PaymentPreference> PaymentPreferences { get; set; }
		public string PlaceOfOrdination { get; set; }
		public string Position { get; set; }
		public Prefix Prefix { get; set; }
		public Guid? PrefixId { get; set; }
		public Profession Profession { get; set; }
		public Guid? ProfessionId { get; set; }
		public DateTime? ProspectDate { get; set; }
		public string Salutation { get; set; }
		public string Source { get; set; }
		public string Suffix { get; set; }
		public DateTime? TaxExemptVerifyDate { get; set; }
		public string TaxId { get; set; }
		public int? YearEstablished { get; set; }
		#endregion Public Properties

		#region Private Properties

		//private IUser UserId { get; set; }

		#endregion Private Properties
	}
}
