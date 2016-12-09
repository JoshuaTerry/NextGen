using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data;
using DDI.Data.Models;
using DDI.Data.Models.Client;
using DDI.Business.Enums;
using System.Data.Entity.Migrations;

namespace DDI.Conversion
{
    class LoadDataCRM
    {

        private const string NACODES_FILE = "nacodes.csv";
        private const string ADDRESSES_FILE = "address.csv";
        private const string PREFIX_FILE = "NamePrefix.csv";
        private const string INDIVIDUAL_CONSTITUENT_FILE = "individual.csv";
        private const string ORGANIZATION_CONSTITUENT_FILE = "organization.csv";
        private const string DOING_BUSINESS_AS_FILE = "constituentDBA.csv";
        private const string EDUCATION_FILE = "EducationInfo.csv";
        private const string ALTERNATE_ID_FILE = "AlternateID.csv";
        private const string CONSTITUENT_ADDRESS_FILE = "ConstituentAddress.csv";
        private const string CONTACT_INFO_FILE = "ContactInfo";

        // nacodes.record-cd sets - these are the ones that are being imported here.
        private const int DENOMINATION_SET = 5;
        //private const int ADDRESS_TYPE_SET = 6;
        private const int CONSTITUENT_TYPE = 7;
        private const int EDUCATION_LEVEL_SET = 8;
        //private const int PROFESSION_SET = 11;
        //private const int EARNINGS_SET = 14;
        private const int LANGUAGE_SET = 26;
        private const int ETHNICITY_SET = 33;
        private const int CLERGY_TYPE_SET = 34;
        private const int CLERGY_STATUS_SET = 35;
        private const int GENDER_SET = 38;
        //private const int MARITAL_STATUS_SET = 39;
        //private const int SCHOOL_SET = 41;
        //private const int DEGREE_SET = 42;


        public void ExecuteCRMLoad(DomainContext context, string organization, string filePath)
        {
            string dataFile = "";


            // Load legacy nacodes
            // - 5 denominations
            // - 7 constituent types
            // - 8 education levels
            // - 26 language
            // - 33 ethnicities
            // - 34 clergy types 
            // - 35 clergy status
            // - 38 genders
            dataFile = filePath + "\\" + organization + "\\CRM\\" + NACODES_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    int codeSet;
                    int.TryParse(parts[1], out codeSet);
                    string code = parts[2];
                    string description = parts[3];
                    int int1;
                    int.TryParse(parts[4], out int1);
                    int int2;
                    int.TryParse(parts[5], out int2);
                    string text1 = parts[6];
                    string text2 = parts[7];
                    string security = parts[8];
                    bool active = Convert.ToBoolean(parts[9] == "yes");
                    string baseType = "";
                    bool required = false;
                    bool masculine = true;

                    switch (codeSet)
                    {
                        case DENOMINATION_SET:
                            context.Denominations.AddOrUpdate(
                                p => p.Code,
                                new Denomination { Code = code, Name = description, Religion = text1, Affiliation = text2, IsActive = active });
                            break;
                        case CONSTITUENT_TYPE:
                            if (code == "I" || code == "F")
                            {
                                baseType = ConstituentCategory.Individual.ToString();
                               
                            }
                            else
                            {
                                baseType = ConstituentCategory.Organization.ToString();
                            }
                            if (code == "I" || code == "O")
                            {
                                required = true;
                            }
                            context.ConstituentTypes.AddOrUpdate(
                                p => p.Code,
                                new ConstituentType { BaseType = baseType, Code = code, Description = description, IsActive = active, IsRequired = required });
                            break;
                        case EDUCATION_LEVEL_SET:
                            context.EducationLevels.AddOrUpdate(
                                p => p.Code,
                                new EducationLevel { Code = code, Name = description, IsActive = active });
                            break;
                        case LANGUAGE_SET:
                            context.Languages.AddOrUpdate(
                               p => p.Code,
                               new Language { Code = code, Name = description, IsActive = active });
                            break;
                        case ETHNICITY_SET:
                            context.Ethnicities.AddOrUpdate(
                               p => p.Code,
                               new Ethnicity { Code = code, Name = description, IsActive = active });
                            break;
                        case CLERGY_TYPE_SET:
                            context.ClergyTypes.AddOrUpdate(
                               p => p.Code,
                               new ClergyType { Code = code, Description = description, IsActive = active });
                            break;
                        case CLERGY_STATUS_SET:
                            context.ClergyStatuses.AddOrUpdate(
                               p => p.Code,
                               new ClergyStatus { Code = code, Name = description, Description = description, IsActive = active });
                            break;
                        case GENDER_SET:
                            if (code == "M")
                            {
                                masculine = true;
                                required = true;
                            }
                            else if (code == "F")
                            {
                                masculine = false;
                                required = true;
                            }
                            context.Genders.AddOrUpdate(
                               p => p.Code,
                               new Gender { Code = code, Name = description, IsMasculine = masculine });
                            break;
                    }

                }
            }

            // Load Addresses
            dataFile = filePath + "\\" + organization + "\\CRM\\" + ADDRESSES_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    int legacyId;
                    int.TryParse(parts[1], out legacyId);
                    string streetAddress = parts[2];
                    string countryCode = parts[3];
                    string stateCode = parts[4];
                    int countyFips;
                    int.TryParse(parts[5], out countyFips);
                    string postalCode = parts[6];
                    string city = parts[7];
                    int region1;
                    int.TryParse(parts[8], out region1);
                    int region2;
                    int.TryParse(parts[9], out region2);
                    int region3;
                    int.TryParse(parts[10], out region3);
                    int region4;
                    int.TryParse(parts[11], out region4);

                    State state = context.States.First(p => p.Abbreviation == stateCode);
                    //TODO: need to assign legacyID to new field in Address table

                    context.Addresses.AddOrUpdate(
                        p => p.Line2,
                        new Address { Line1 = streetAddress, Line2 = legacyId.ToString(), City = city, State = state, StateId = state.Id, Zip = postalCode });

                }
            }

            // Load prefixes
            dataFile = filePath + "\\" + organization + "\\CRM\\" + PREFIX_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    string prefix = parts[1];
                    string salutation = parts[2];
                    string label = parts[3];
                    string gender = parts[4];
                    string prior = parts[5];
                    string labelAbbreviation = parts[6];
                    bool isWebAvailable = (parts[7] == "yes");

                    Gender g1 = context.Genders.First(p => p.Code == gender);

                    context.Prefixes.AddOrUpdate(
                        p => p.Abbreviation,
                        new Prefix { Abbreviation = labelAbbreviation, Descriptin = label, Gender = g1, GenderId = g1.Id });

                }
            }

            // Load constituents and constituent denomination cross reference and constituent ethnicity - was 1 to 1 in legacy app
            dataFile = filePath + "\\" + organization + "\\CRM\\" + INDIVIDUAL_CONSTITUENT_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    int constituentId;
                    int.TryParse(parts[1], out constituentId);
                    string constituentType = parts[2];
                    string name = parts[3];
                    string name2 = parts[4];
                    string sourceCode = parts[5];
                    string taxId = parts[6];
                    string ethnicity = parts[7];
                    string denomination = parts[8];
                    string correspondencePreference = parts[9];
                    string salutationFormat = parts[10];
                    string salutationText = parts[11];
                    string prefix = parts[12];
                    string firstName = parts[13];
                    string middleName = parts[14];
                    string lastName = parts[15];
                    string suffix = parts[16];
                    string nickname = parts[17];
                    string nameFormat = parts[18];
                    string gender = parts[19];
                    string earnings = parts[20];
                    string educationLevel = parts[21];
                    string employer = parts[22];
                    string position = parts[23];
                    DateTime employmentStartDate;
                    DateTime.TryParse(parts[24], out employmentStartDate);
                    DateTime employmentEndDate;
                    DateTime.TryParse(parts[25], out employmentEndDate);
                    DateTime firstEmploymentDate;
                    DateTime.TryParse(parts[26], out firstEmploymentDate);
                    bool isClientEmployee = (parts[27] == "yes");
                    string profession = parts[28];
                    string clergyType = parts[29];
                    string clergyStatus = parts[30];
                    DateTime ordinationDate;
                    DateTime.TryParse(parts[31], out ordinationDate);
                    string ordinationPlace = parts[32];
                    DateTime prospectDate;
                    DateTime.TryParse(parts[33], out prospectDate);
                    string maritalStatus = parts[34];
                    DateTime marriageDate;
                    DateTime.TryParse(parts[35], out marriageDate);
                    DateTime divorceDate;
                    DateTime.TryParse(parts[36], out divorceDate);
                    DateTime deceasedDate;
                    DateTime.TryParse(parts[37], out deceasedDate);
                    int birthMonth;
                    int.TryParse(parts[38], out birthMonth);
                    int birthDay;
                    int.TryParse(parts[39], out birthDay);
                    int birthYear1;
                    int.TryParse(parts[40], out birthYear1);
                    int birthYear2;
                    int.TryParse(parts[41], out birthYear2);
                    string deletionCode = parts[42];
                    DateTime deletionDate;
                    DateTime.TryParse(parts[43], out deletionDate);

                    Gender g1 = context.Genders.First(p => p.Code == gender);
                    ClergyStatus cs1 = context.ClergyStatuses.First(p => p.Code == clergyStatus);
                    ClergyType ct1 = context.ClergyTypes.First(p => p.Code == clergyType);
                    ConstituentStatus cs2;
                    if (deletionDate == null)
                    {
                        cs2 = context.ConstituentStatuses.First(p => p.Code == "AC");
                    }
                    else
                    {
                        cs2 = context.ConstituentStatuses.First(p => p.Code == "IN");
                    }
                    ConstituentType ct2 = context.ConstituentTypes.First(p => p.Code == constituentType);
                    EducationLevel e1 = context.EducationLevels.First(p => p.Code == educationLevel);
                    string formattedName = firstName;
                    if (middleName != null)
                    {
                        formattedName = formattedName + " " + middleName;
                    }
                    if(lastName != null)
                    {
                        formattedName = formattedName + " " + lastName;
                    }
                    DateTime birthDate;
                    if (birthMonth != 0 && birthDay != 0 && birthYear1 != 0)
                    {
                        birthDate = new DateTime(birthYear1, birthMonth, birthDay);
                    }
                    else
                    {
                        birthDate = new DateTime(1000,1,1);
                    }
                    IncomeLevel il1 = context.IncomeLevels.First(p => p.Code == earnings);
                    Prefix p1 = context.Prefixes.First(p => p.Abbreviation == prefix);
                    Profession p2 = context.Professions.First(p => p.Code == profession);
                    int ms1;
                    int.TryParse(maritalStatus, out ms1);
                    //TODO: run salutation through the salutation logic when it gets there if salutation text is not filled in.
                   

                    //TODO: create constituent
                    context.Constituents.AddOrUpdate(
                        p => p.ConstituentNum,
                        new Constituent { BirthDate = birthDate, BirthYearFrom = birthYear1, BirthYearTo = birthYear2, Business = profession, ClergyStatus = cs1, ClergyStatusId = cs1.Id,
                            ClergyType = ct1, ClergyTypeId = ct1.Id, ConstituentStatus = cs2, ConstituentStatusId = cs2.Id, ConstituentNum = constituentId,
                            ConstituentType = ct2, ConstituentTypeId = ct2.Id, DeceasedDate = deceasedDate, DivorceDate = divorceDate, EducationLevel = e1, Employer = employer,
                            EmploymentStartDate = employmentStartDate, EmploymentEndDate = employmentEndDate, FirstEmploymentDate = firstEmploymentDate, FirstName = firstName,
                            FormattedName =  formattedName, IncomeLevel = il1, IncomeLevelId = il1.Id, IsEmployee = isClientEmployee, LastName = lastName, MaritalStatus = ms1,
                            MarriageDate = marriageDate, MiddleName = middleName, Name2 = name2, Nickname = nickname, OrdinationDate = ordinationDate, PlaceOfOrdination = ordinationPlace,
                            Position = position, Prefix = p1, PrefixId = p1.Id, Profession = p2, ProfessionId = p2.Id, ProspectDate = prospectDate, Salutation = salutationText,
                            Source = sourceCode, Suffix = suffix, TaxId = taxId});

                    

                }
            }

            // Load Doing BusinessAs

            // Load Education

            // Load Payment Preferences - may be in a different module

            // Load AleternatIDs

            // Load Constituent Address cross reference
            dataFile = filePath + "\\" + organization + "\\CRM\\" + CONSTITUENT_ADDRESS_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    int legacyAddressId;
                    int.TryParse(parts[1], out legacyAddressId);
                    int constituentNum;
                    int.TryParse(parts[2], out constituentNum);
                    string addressTypeCode = parts[3];
                    string comment = parts[4];
                    DateTime startDate;
                    DateTime.TryParse(parts[5], out startDate);
                    DateTime endDate;
                    DateTime.TryParse(parts[6], out endDate);
                    int startDay;
                    int.TryParse(parts[7], out startDay);
                    int endDay;
                    int.TryParse(parts[8], out endDay);
                    bool isPrimary = (parts[9] == "yes");
                    string residentType = parts[10];

                    Address a1 = context.Addresses.First(p => p.Line2 == legacyAddressId.ToString());
                    Constituent c1 = context.Constituents.First(p => p.ConstituentNum == constituentNum);

                    context.ConstituentAddresses.AddOrUpdate(
                        p => p.Id,
                        new ConstituentAddress { Address = a1, AddressId = a1.Id, Constituent = c1, ConstituentId = c1.Id, IsPrimary = isPrimary });

                }
            }
            // Load contact info

        }
    }
}
