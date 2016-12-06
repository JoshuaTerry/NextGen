using DDI.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DDI.Business.DataModels
{
    [Table("Constituent")]
    public class Constituent : IConstituent
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
       public int ConstituentNum { get; set; }
        
       public IConstituentType ConstituentType { get; set; }
        
       public IPrefix Prefix { get; set; }
        
       public string FirstName { get; set; }
        
       public string MiddleName { get; set; }
        
       public string LastName { get; set; }
        
       public string Suffix { get; set; }
        
       public string Name2 { get; set; }
        
       public string Nickname { get; set; }
        
       public string FormattedName { get; set; }
        
       public string Salutation { get; set; }
        
       public string TaxId { get; set; }
        
       public IGender Gender { get; set; }
        
       public string Source { get; set; }
        
       public IConstituentStatus ConstituentStatus { get; set; }
        
       public IClergyType ClergyType { get; set; }
        
       public IClergyStatus ClergyStatusID { get; set; }
        
       public DateTime? OrdinationDate { get; set; }
        
       public string PlaceOfOrdination { get; set; }
        
       public ILanguage LanguageId { get; set; }
        
        //Question - Not sure I understand what this is for
        //If its to determine the highest level of education 
        //We can just use an Education Collection and get Max
       public IEducationLevel EducationLevel { get; set; }
        
       public int? PreferredPaymentMethod { get; set; }
        
       public DateTime? BirthDate { get; set; }
        
       public int? BirthYearFrom { get; set; }
        
       public int? BirthYearTo { get; set; }
        
       public DateTime? DeceasedDate { get; set; }
        
       public int? MaritalStatus { get; set; }
        
       public DateTime? MarriageDate { get; set; }
        
       public DateTime? DivorceDate { get; set; }
        
       public DateTime? ProspectDate { get; set; }
        
       public IProfession Profession { get; set; }
        
       public IIncomeLevel IncomeLevel { get; set; }
        
       public DateTime? FirstEmploymentDate { get; set; }
        
       public string Employer { get; set; }
        
       public string Position { get; set; }
        
       public DateTime? EmploymentStartDate { get; set; }
        
       public DateTime? EmploymentEndDate { get; set; }
        
       public bool IsEmployee { get; set; }
        
        //Question I dunno that we need Users and Constituents joined like this anymore
        //User? UserID { get; set; }
        
       public int? MembershipCount { get; set; }
        
       public int? YearEstablished { get; set; }
      
       public string Business { get; set; }
 
       public bool IsTaxExempt { get; set; }
      
       public DateTime? TaxExemptVerifyDate { get; set; }
 
       public bool IsIRSLetterReceived { get; set; }
   
       public ICollection<IAddress> Addresses { get; set; }
     
       public ICollection<IDenomination> Denominations { get; set; }
       
       public ICollection<IEthnicity> Ethnicities { get; set; }
       
       public ICollection<IDoingBusinessAs> DoingBusinessAs { get; set; }
      
       public ICollection<IEducation> Educations { get; set; }

       public ICollection<IPaymentPreference> PaymentPreferences { get; set; }

       public ICollection<IAlternateId> AlternateIds { get; set; }
    
       public ICollection<IContactInfo> ContactInfo { get; set; }
    }
}