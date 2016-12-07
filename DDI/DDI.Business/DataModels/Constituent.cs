using DDI.Shared;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("Constituent")]
    public class Constituent 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
       public int ConstituentNum { get; set; }
        public Guid? ConstituentTypeId { get; set; }
        public ConstituentType ConstituentType { get; set; }
        
       public Guid? PrefixId { get; set; }
       public Prefix Prefix { get; set; }
        
       public string FirstName { get; set; }
        
       public string MiddleName { get; set; }
        
       public string LastName { get; set; }
        
       public string Suffix { get; set; }
        
       public string Name2 { get; set; }
        
       public string Nickname { get; set; }
        
       public string FormattedName { get; set; }
        
       public string Salutation { get; set; }
        
       public string TaxId { get; set; }
        
       public Guid? GenderId { get; set; }

        public Gender Gender { get; set; }
        
       public string Source { get; set; }

       public Guid? ConstituentStatusId { get; set; }

        public ConstituentStatus ConstituentStatus { get; set; }

        public Guid? ClergyTypeId { get; set; }

        public ClergyType ClergyType { get; set; }

        public Guid? ClergyStatusId { get; set; }

        public ClergyStatus ClergyStatus { get; set; }
        
       public DateTime? OrdinationDate { get; set; }
        
       public string PlaceOfOrdination { get; set; }

        public Guid? LanguageId { get; set; }

        public Language Language { get; set; }
        
       public EducationLevel EducationLevel { get; set; }
        
       public int? PreferredPaymentMethod { get; set; }
        
       public DateTime? BirthDate { get; set; }
        
       public int? BirthYearFrom { get; set; }
        
       public int? BirthYearTo { get; set; }
        
       public DateTime? DeceasedDate { get; set; }
        
       public int? MaritalStatus { get; set; }
        
       public DateTime? MarriageDate { get; set; }
        
       public DateTime? DivorceDate { get; set; }
        
       public DateTime? ProspectDate { get; set; }

        public Guid? ProfessionId { get; set; }

        public Profession Profession { get; set; }

        public Guid? IncomeLevelId { get; set; }

        public IncomeLevel IncomeLevel { get; set; }
        
       public DateTime? FirstEmploymentDate { get; set; }
        
       public string Employer { get; set; }
        
       public string Position { get; set; }
        
       public DateTime? EmploymentStartDate { get; set; }
        
       public DateTime? EmploymentEndDate { get; set; }
        
       public bool IsEmployee { get; set; }
         
       IUser UserId { get; set; }
        
       public int? MembershipCount { get; set; }
        
       public int? YearEstablished { get; set; }
      
       public string Business { get; set; }
 
       public bool IsTaxExempt { get; set; }
      
       public DateTime? TaxExemptVerifyDate { get; set; }
 
       public bool IsIRSLetterReceived { get; set; }
   
       public virtual ICollection<Address> Addresses { get; set; }
     
       public virtual ICollection<Denomination> Denominations { get; set; }
       
       public virtual ICollection<Ethnicity> Ethnicities { get; set; }
       
       public virtual ICollection<DoingBusinessAs> DoingBusinessAs { get; set; }
      
       public virtual ICollection<Education> Educations { get; set; }

       public virtual ICollection<PaymentPreference> PaymentPreferences { get; set; }

       public virtual ICollection<AlternateId> AlternateIds { get; set; }
    
       public virtual ICollection<ContactInfo> ContactInfo { get; set; }
    }
}