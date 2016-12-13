using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Data;
using DDI.Data.Models;
using DDI.Data.Models.Client;
using DDI.Data.Models.Common;
using DDI.Data.Enums;
using System.Data.Entity.Migrations;

namespace DDI.Conversion
{
    class LoadDataCRM : DbMigrationsConfiguration<DomainContext>
    {

        private const string NACODES_FILE = "NACodes.csv";
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
        private const int STATE_SET = 10;
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


        public static void ExecuteCRMLoad(DomainContext context, CommonContext common, string organization, string filePath)
        {
            string dataFile = "";


            //// Load legacy nacodes
            //// - 5 denominations
            //// - 7 constituent types
            //// - 8 education levels
            //// - 10 states
            //// - 26 language
            //// - 33 ethnicities
            //// - 34 clergy types 
            //// - 35 clergy status
            //// - 38 genders
            //dataFile = filePath + "\\" + organization + "\\CRM\\" + NACODES_FILE;
            //using (System.IO.StreamReader sr = new StreamReader(dataFile))
            //{
            //    String line;

            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        int codeSet;
            //        string code = null;
            //        string description = null;
            //        int int1;
            //        int int2;
            //        string text1 = null;
            //        string text2 = null;
            //        string security = null;
            //        bool active = false;
            //        string baseType = null;
            //        bool required = false;
            //        bool masculine = true;

            //        Console.WriteLine(line);
            //        string[] parts = ParseLine(line);
            //        int.TryParse(parts[0], out codeSet);
            //        if (parts.Length >= 2)
            //        {
            //            code = parts[1];
            //        }
            //        if (parts.Length >= 3)
            //        {
            //            description = parts[2];
            //        }
            //        if (parts.Length >= 4)
            //        {
            //            int.TryParse(parts[3], out int1);
            //        }
            //        if (parts.Length >= 5)
            //        {
            //            int.TryParse(parts[4], out int2);
            //        }
            //        if (parts.Length >= 6)
            //        {
            //            text1 = parts[5];
            //        }
            //        if (parts.Length >= 7)
            //        {
            //            text2 = parts[6];
            //        }
            //        if (parts.Length >= 8)
            //        {
            //            security = parts[7];
            //        }
            //        if (parts.Length >= 9)
            //        {
            //            active = (parts[8] == "yes");
            //        }

            //        switch (codeSet)
            //        {
            //            case DENOMINATION_SET:
            //                context.Denominations.AddOrUpdate(
            //                    p => p.Code,
            //                    new Denomination { Code = code, Name = description, Religion = text1, Affiliation = text2, IsActive = active });
            //                break;
            //            case CONSTITUENT_TYPE:
            //                if (code == "I" || code == "F")
            //                {
            //                    baseType = ConstituentCategory.Individual.ToString();

            //                }
            //                else
            //                {
            //                    baseType = ConstituentCategory.Organization.ToString();
            //                }
            //                if (code == "I" || code == "O")
            //                {
            //                    required = true;
            //                }
            //                context.ConstituentTypes.AddOrUpdate(
            //                    p => p.Code,
            //                    new ConstituentType { BaseType = baseType, Code = code, Description = description, IsActive = active, IsRequired = required });
            //                break;
            //            case EDUCATION_LEVEL_SET:
            //                context.EducationLevels.AddOrUpdate(
            //                    p => p.Code,
            //                    new EducationLevel { Code = code, Name = description, IsActive = active });
            //                break;
            //            //case STATE_SET:
            //            //    context.States.AddOrUpdate(
            //            //        p => p.StateCode,
            //            //        new State { StateCode = code, Description = description });
            //            //    break;
            //            case LANGUAGE_SET:
            //                context.Languages.AddOrUpdate(
            //                   p => p.Code,
            //                   new Language { Code = code, Name = description, IsActive = active });
            //                break;
            //            case ETHNICITY_SET:
            //                context.Ethnicities.AddOrUpdate(
            //                   p => p.Code,
            //                   new Ethnicity { Code = code, Name = description, IsActive = active });
            //                break;
            //            case CLERGY_TYPE_SET:
            //                context.ClergyTypes.AddOrUpdate(
            //                   p => p.Code,
            //                   new ClergyType { Code = code, Description = description, IsActive = active });
            //                break;
            //            case CLERGY_STATUS_SET:
            //                context.ClergyStatuses.AddOrUpdate(
            //                   p => p.Code,
            //                   new ClergyStatus { Code = code, Name = description, Description = description, IsActive = active });
            //                break;
            //            case GENDER_SET:
            //                if (code == "M")
            //                {
            //                    masculine = true;
            //                    required = true;
            //                }
            //                else if (code == "F")
            //                {
            //                    masculine = false;
            //                    required = true;
            //                }
            //                context.Genders.AddOrUpdate(
            //                   p => p.Code,
            //                   new Gender { Code = code, Name = description, IsMasculine = masculine });
            //                break;
            //        }

            //    }
            //}

            //            // Load Addresses
            //            dataFile = filePath + "\\" + organization + "\\CRM\\" + ADDRESSES_FILE;
            //            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            //            {
            //                String line;
            //                int count = 0;

            //                while (count <= 2000 && (line = sr.ReadLine()) != null)
            //                {
            //                    int legacyId;
            //                    string streetAddress = null;
            //                    string countryCode = null;
            //                    string stateCode = null;
            //                    int countyFips = 0;
            //                    string postalCode = null;
            //                    string city = null;
            //                    string line2;
            //                    count++;

            //                    string[] parts = ParseLine(line);
            //                    Console.WriteLine(line);
            //                    if (parts.Length >= 7)
            //                    {
            //                        int.TryParse(parts[0], out legacyId);
            //                        streetAddress = parts[1];
            //                        //countryCode = parts[2];
            //                        stateCode = parts[3];
            //                        int.TryParse(parts[4], out countyFips);
            //                        postalCode = parts[5];
            //                        city = parts[6];
            //                        //int region1;
            //                        //int.TryParse(parts[7], out region1);
            //                        //int region2;
            //                        //int.TryParse(parts[8], out region2);
            //                        //int region3;
            //                        //int.TryParse(parts[9], out region3);
            //                        //int region4;
            //                        //int.TryParse(parts[10], out region4);
            //                        line2 = legacyId.ToString();
            //                        stateCode = stateCode.Trim('"');
            //                        //Console.WriteLine(stateCode);
            //                        //Guid countryId = new Guid("d21b95c7-e45b-4cf7-83fe-3fff31436ddf");

            ////                        if (legacyId < 20000 && stateCode != null && stateCode != "")
            //                            if (legacyId < 20000)

            //                            {
            //                                //State state = common.States.FirstOrDefault(p => p.StateCode == stateCode && p.CountryId == countryId);
            //                            //Console.WriteLine(state.Description);
            //                            //Country country = context.Country.FirstOrDefault(parts => parts.)
            //                            //TODO: need to assign legacyID to new field in Address table
            //                            //if (state != null)
            //                            //{
            //                                context.Addresses.AddOrUpdate(
            //                                    p => p.Line2,
            //                                    //new Address { Line1 = streetAddress, Line2 = line2, City = city, State = state, StateId = state.Id, Zip = postalCode });
            //                                    new Address { Line1 = streetAddress, Line2 = line2, City = city, Zip = postalCode });
            //                            //}
            //                        }
            //                    }
            //                }
            //            }

            //// Load prefixes
            //dataFile = filePath + "\\" + organization + "\\CRM\\" + PREFIX_FILE;
            //using (System.IO.StreamReader sr = new StreamReader(dataFile))
            //{
            //    String line;

            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] parts = ParseLine(line);
            //        string prefix = parts[0];
            //        string salutation = parts[1];
            //        string label = parts[2];
            //        string gender = parts[3];
            //        string prior = parts[4];
            //        string labelAbbreviation = parts[5];
            //        bool isWebAvailable = (parts[6] == "yes");
            //        Gender g1 = null;

            //        if (gender != null)
            //        {
            //            g1 = context.Genders.FirstOrDefault(p => p.Code == gender);
            //        }
            //        if (g1 != null)
            //        {
            //            context.Prefixes.AddOrUpdate(
            //                p => p.Abbreviation,
            //                new Prefix { Abbreviation = labelAbbreviation, Description = label, Gender = g1, GenderId = g1.Id });
            //        }
            //        else
            //        {
            //            context.Prefixes.AddOrUpdate(
            //                p => p.Abbreviation,
            //                new Prefix { Abbreviation = labelAbbreviation, Description = label });
            //        }


            //    }
            //}

            //// Load constituents and constituent denomination cross reference and constituent ethnicity - was 1 to 1 in legacy app
            //dataFile = filePath + "\\" + organization + "\\CRM\\" + INDIVIDUAL_CONSTITUENT_FILE;
            //using (System.IO.StreamReader sr = new StreamReader(dataFile))
            //{
            //    String line;
            //    int count = 0;

            //    while (count <= 1000 && (line = sr.ReadLine()) != null)
            //    {
            //        count++;
            //        string[] parts = ParseLine(line);
            //        int constituentId;
            //        int.TryParse(parts[0], out constituentId);
            //        string constituentType = parts[1];
            //        string name = parts[2].Trim('"');
            //        string name2 = parts[3].Trim('"');
            //        string sourceCode = parts[4];
            //        string taxId = parts[5];
            //        string ethnicity = parts[6];
            //        string denomination = parts[7];
            //        string correspondencePreference = parts[8];
            //        string salutationFormat = parts[9];
            //        string salutationText = parts[10];
            //        string prefix = parts[11];
            //        string firstName = parts[12].Trim('"');
            //        string middleName = parts[13].Trim('"');
            //        string lastName = parts[14].Trim('"');
            //        string suffix = parts[15];
            //        string nickname = parts[16];
            //        string nameFormat = parts[17];
            //        string gender = parts[18];
            //        string earnings = parts[19];
            //        string educationLevel = parts[20];
            //        string employer = parts[21];
            //        string position = parts[22];
            //        DateTime employmentStartDate;
            //        DateTime.TryParse(parts[23], out employmentStartDate);
            //        DateTime employmentEndDate;
            //        DateTime.TryParse(parts[24], out employmentEndDate);
            //        DateTime firstEmploymentDate;
            //        DateTime.TryParse(parts[25], out firstEmploymentDate);
            //        bool isClientEmployee = (parts[26] == "yes");
            //        string profession = parts[27];
            //        string clergyType = parts[28];
            //        string clergyStatus = parts[29];
            //        DateTime ordinationDate;
            //        DateTime.TryParse(parts[30], out ordinationDate);
            //        string ordinationPlace = parts[31];
            //        DateTime prospectDate;
            //        DateTime.TryParse(parts[32], out prospectDate);
            //        string maritalStatus = parts[33];
            //        DateTime marriageDate;
            //        DateTime.TryParse(parts[34], out marriageDate);
            //        DateTime divorceDate;
            //        DateTime.TryParse(parts[35], out divorceDate);
            //        DateTime deceasedDate;
            //        DateTime.TryParse(parts[36], out deceasedDate);
            //        int birthMonth;
            //        int.TryParse(parts[37], out birthMonth);
            //        int birthDay;
            //        int.TryParse(parts[38], out birthDay);
            //        int birthYear1;
            //        int.TryParse(parts[39], out birthYear1);
            //        int birthYear2;
            //        int.TryParse(parts[40], out birthYear2);
            //        string deletionCode = parts[41];
            //        DateTime deletionDate;
            //        DateTime.TryParse(parts[42], out deletionDate);

            //        //if (gender != null)
            //        //{ Gender g1 = context.Genders.First(p => p.Code == gender); }
            //        //if (clergyStatus != null)
            //        //{ ClergyStatus cs1 = context.ClergyStatuses.First(p => p.Code == clergyStatus); }
            //        //if (clergyType != null)
            //        //{ ClergyType ct1 = context.ClergyTypes.First(p => p.Code == clergyType); }
            //        ConstituentStatus cs2;
            //        if (deletionDate == null)
            //        {
            //            cs2 = context.ConstituentStatuses.First(p => p.Code == "AC");
            //        }
            //        else
            //        {
            //            cs2 = context.ConstituentStatuses.First(p => p.Code == "IN");
            //        }
            //        ConstituentType ct2 = null;
            //        if (constituentType != null)
            //        {
            //            constituentType = constituentType.Trim('"');
            //            //constituentType = constituentType.Trim(;
            //            ct2 = context.ConstituentTypes.First(p => p.Code == constituentType); }
            //        //EducationLevel e1 = context.EducationLevels.First(p => p.Code == educationLevel);
            //        string formattedName = firstName.Trim('"');
            //        if (middleName != null)
            //        {
            //            formattedName = formattedName + " " + middleName.Trim('"');
            //        }
            //        if (lastName != null)
            //        {
            //            formattedName = formattedName + " " + lastName.Trim('"');
            //        }
            //        //DateTime birthDate;
            //        //if (birthMonth != 0 && birthDay != 0 && birthYear1 != 0)
            //        //{
            //        //    birthDate = new DateTime(birthYear1, birthMonth, birthDay);
            //        //}
            //        //else
            //        //{
            //        //    birthDate = new DateTime(1000, 1, 1);
            //        //}
            //        //IncomeLevel il1 = context.IncomeLevels.First(p => p.Code == earnings);
            //        //Prefix p1 = context.Prefixes.First(p => p.Abbreviation == prefix);
            //        //Profession p2 = context.Professions.First(p => p.Code == profession);
            //        //int ms1;
            //        //int.TryParse(maritalStatus, out ms1);
            //        //TODO: run salutation through the salutation logic when it gets there if salutation text is not filled in.
            //        //Console.WriteLine(birthYear1);
            //        //Console.WriteLine(birthYear2);
            //        //Console.WriteLine(cs2.Id);
            //        Console.WriteLine(constituentId);
            //        //Console.WriteLine(ct2.Id);
            //        //Console.WriteLine(deceasedDate);
            //        //Console.WriteLine(divorceDate);
            //        //Console.WriteLine(employer);
            //        //Console.WriteLine(employmentStartDate);
            //        //Console.WriteLine(employmentEndDate);
            //        //Console.WriteLine(firstEmploymentDate);
            //        //Console.WriteLine(firstName);
            //        //Console.WriteLine(formattedName);
            //        //Console.WriteLine(isClientEmployee);
            //        //Console.WriteLine(lastName);
            //        //Console.WriteLine(marriageDate);
            //        //Console.WriteLine(middleName);
            //        //Console.WriteLine(name2);
            //        //Console.WriteLine(nickname);
            //        //Console.WriteLine(ordinationDate);
            //        //Console.WriteLine(ordinationPlace);
            //        //Console.WriteLine(position);
            //        //Console.WriteLine(prospectDate);
            //        //Console.WriteLine(salutationText);
            //        //Console.WriteLine(sourceCode);
            //        //Console.WriteLine(suffix);
            //        //Console.WriteLine(taxId);

            //        //TODO: create constituent
            //        context.Constituents.AddOrUpdate(
            //            p => p.ConstituentNum,
            //            new Constituent
            //            {
            //                //BirthDate = birthDate,
            //                BirthYearFrom = birthYear1,
            //                BirthYearTo = birthYear2,
            //                Business = profession,
            //                //ClergyStatus = cs1,
            //                //ClergyStatusId = cs1.Id,
            //                //ClergyType = ct1,
            //                //ClergyTypeId = ct1.Id,
            //                //ConstituentStatus = cs2,
            //                //ConstituentStatusId = cs2.Id,
            //                ConstituentNum = constituentId,
            //                ConstituentType = ct2,
            //                ConstituentTypeId = ct2.Id,
            //                //DeceasedDate = deceasedDate,
            //                //DivorceDate = divorceDate,
            //                //EducationLevel = e1,
            //                Employer = employer,
            //                //EmploymentStartDate = employmentStartDate,
            //                //EmploymentEndDate = employmentEndDate,
            //                //FirstEmploymentDate = firstEmploymentDate,
            //                FirstName = firstName,
            //                FormattedName = formattedName,
            //                //IncomeLevel = il1,
            //                //IncomeLevelId = il1.Id,
            //                IsEmployee = isClientEmployee,
            //                LastName = lastName,
            //                //MaritalStatus = ms1,
            //                //MarriageDate = marriageDate,
            //                MiddleName = middleName,
            //                Name2 = name2,
            //                Nickname = nickname,
            //                //OrdinationDate = ordinationDate,
            //                PlaceOfOrdination = ordinationPlace,
            //                Position = position,
            //                //Prefix = p1,
            //                //PrefixId = p1.Id,
            //                //Profession = p2,
            //                //ProfessionId = p2.Id,
            //                //ProspectDate = prospectDate,
            //                Salutation = salutationText,
            //                Source = sourceCode,
            //                Suffix = suffix,
            //                TaxId = taxId
            //            });



            //    }
            //}

            //// Load Doing BusinessAs

            //// Load Education

            //// Load Payment Preferences - may be in a different module

            //// Load AleternatIDs

            // Load Constituent Address cross reference
            dataFile = filePath + "\\" + organization + "\\CRM\\" + CONSTITUENT_ADDRESS_FILE;
            using (System.IO.StreamReader sr = new StreamReader(dataFile))
            {
                String line;
                int count = 0;

                while (count <= 2000 && (line = sr.ReadLine()) != null)
                {
                    count++;
                    Console.WriteLine(line);
                    string[] parts = ParseLine(line);
                    int legacyAddressId = 0;
                    int.TryParse(parts[0], out legacyAddressId);
                    int constituentNum = 0;
                    int.TryParse(parts[1], out constituentNum);
                    //string addressTypeCode = parts[2];
                    //string comment = parts[3];
                    //DateTime startDate;
                    //DateTime.TryParse(parts[4], out startDate);
                    //DateTime endDate;
                    //DateTime.TryParse(parts[5], out endDate);
                    //int startDay;
                    //int.TryParse(parts[6], out startDay);
                    //int endDay;
                    //int.TryParse(parts[7], out endDay);
                    bool isPrimary = (parts[8] == "yes");
                    //string residentType = parts[9];
                    Console.WriteLine("{0}: {1} {2}", count, legacyAddressId, constituentNum);

                    Address a1 = null;
                    Constituent c1 = null;
                    if (legacyAddressId != 0 ) 
                    {
                        try
                        {
                            a1 = context.Addresses.First(p => p.Line2 == legacyAddressId.ToString());
                        }
                        catch (Exception e)
                        { }
                    }
                    if (constituentNum != 0)
                    {
                        try
                        {
                            c1 = context.Constituents?.First(p => p.ConstituentNum == constituentNum);
                        }
                        catch (Exception e)
                        {
                            //don't do anything, just keep going
                        }
                    }

                    if (a1 != null && c1 != null)
                    {
                        
                        context.ConstituentAddresses.AddOrUpdate(
                            p => p.Id,
                            new ConstituentAddress { Address = a1, AddressId = a1.Id, Constituent = c1, ConstituentId = c1.Id, IsPrimary = isPrimary });
                    }
                }
            }

            //// Load contact info

        }
        public static string[] ParseLine(string line)
        {
            string[] parts = line.Split(',');

            int i = 0;
            while (i < parts.Length - 1)
            {
                if (parts[i] != null && parts[i] != "")
                {
                    if (parts[i].First() == '"' && parts[i].Last() != '"')
                    {
                        parts[i] = parts[i] + "," + parts[i + 1];

                        int j = i+1;
                        while (j < parts.Length - 1)
                        {
                            parts[j] = parts[j+1];
                            j++;
                        }
                    }
                }
                i++;

            }

            return parts;
        }
    }
}
