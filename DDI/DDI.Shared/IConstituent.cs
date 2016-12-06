using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Shared
{
    public interface IConstituent
    {
        Guid Id { get; set; }

        int ConstituentNum { get; set; }

        IConstituentType ConstituentType { get; set; }

        IPrefix Prefix { get; set; }
        
        string FirstName { get; set; }
        
        string MiddleName { get; set; }
                
        string LastName { get; set; }

        string Suffix { get; set; }

        string Name2 { get; set; }

        string Nickname { get; set; }

        string FormattedName { get; set; }

        string Salutation { get; set; }

        string TaxId { get; set; }

        IGender Gender { get; set; }

        string Source { get; set; }

        IConstituentStatus ConstituentStatus { get; set; }

        IClergyType ClergyType { get; set; }

        IClergyStatus ClergyStatusID { get; set; }

        DateTime? OrdinationDate { get; set; }

        string PlaceOfOrdination { get; set; }

        ILanguage LanguageId { get; set; }

        //Question - Not sure I understand what this is for
        //If its to determine the highest level of education 
        //We can just use an Education Collection and get Max
        IEducationLevel EducationLevel { get; set; }

        int? PreferredPaymentMethod { get; set; }

        DateTime? BirthDate { get; set; }

        int? BirthYearFrom { get; set; }

        int? BirthYearTo { get; set; }

        DateTime? DeceasedDate { get; set; }

        int? MaritalStatus { get; set; }

        DateTime? MarriageDate { get; set; }

        DateTime? DivorceDate { get; set; }

        DateTime? ProspectDate { get; set; }

        IProfession Profession { get; set; }

        IIncomeLevel IncomeLevel { get; set; }

        DateTime? FirstEmploymentDate { get; set; }

        string Employer { get; set; }

        string Position { get; set; }

        DateTime? EmploymentStartDate { get; set; }

        DateTime? EmploymentEndDate { get; set; }

        bool IsEmployee { get; set; }

        //Question I dunno that we need Users and Constituents joined like this anymore
        //User? UserID { get; set; }

        int? MembershipCount { get; set; }

        int? YearEstablished { get; set; }

        string Business { get; set; }

        bool IsTaxExempt { get; set; }

        DateTime? TaxExemptVerifyDate { get; set; }

        bool IsIRSLetterReceived { get; set; }

        ICollection<IAddress> Addresses { get; set; }

        ICollection<IDenomination> Denominations { get; set; }

        ICollection<IEthnicity> Ethnicities { get; set; }

        ICollection<IDoingBusinessAs> DoingBusinessAs { get; set; }

        ICollection<IEducation> Educations { get; set; }

        ICollection<IPaymentPreference> PaymentPreferences { get; set; }

        ICollection<IAlternateId> AlternateIds { get; set; }

        ICollection<IContactInfo> ContactInfos { get; set; }
    }
}
