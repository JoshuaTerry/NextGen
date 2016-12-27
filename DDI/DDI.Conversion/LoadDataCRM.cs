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
        private const int ADDRESS_TYPE_SET = 6;
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
        private const int CONTACT_TYPE = 71;




        public static void ExecuteCRMLoad(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {
            //filePath = filePath + "\\" + organization + "\\CRM\\";
            filePath = Path.Combine(filePath, organization, "CRM");

            //LoadLegacyCodes(context, common, organization, filePath, minCount, maxCount);
            //LoadAddresses(context, common, organization, filePath, minCount, maxCount);
            //LoadPrefixes(context, common, organization, filePath, minCount, maxCount);
            //LoadConstituents(context, common, organization, filePath, minCount, maxCount);
            //LoadDoingBusinessAs(context, common, organization, filePath, minCount, maxCount);
            //LoadEducation(context, common, organization, filePath, minCount, maxCount);
            //LoadPaymentPreferences(context, common, organization, filePath, minCount, maxCount);
            //LoadAleternateIDs(context, common, organization, filePath, minCount, maxCount);
            LoadConstituentAddress(context, common, organization, filePath, minCount, maxCount);
            //LoadContactInfo(context, common, organization, filePath, minCount, maxCount);

        }

        private static void LoadLegacyCodes(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {
            string dataFile = "";
            
            // - 5 denominations
            // - 7 constituent types
            // - 8 education levels
            // - 10 states -> part of the common database, not being converted
            // - 26 language
            // - 33 ethnicities
            // - 34 clergy types 
            // - 35 clergy status
            // - 38 genders

            //dataFile = filePath + NACODES_FILE;
            dataFile = Path.Combine(filePath, NACODES_FILE);
            using (var importer = new FileImport(dataFile, "NACodes"))
            {
                while (importer.GetNextRow())
                {
                    int codeSet = importer.GetInt(0);
                    string code = importer.GetString(1);
                    string description = importer.GetString(2);
                    int int1 = importer.GetInt(3);
                    int int2 = importer.GetInt(4);
                    string text1 = importer.GetString(5);
                    string text2 = importer.GetString(6);
                    string security = importer.GetString(7);
                    bool active = importer.GetBool(8);
                    string baseType = importer.GetString(9);
                    bool required = false;
                    bool masculine = true;
                    
                    switch (codeSet)
                    {
                        case ADDRESS_TYPE_SET:
                            context.AddressTypes.AddOrUpdate(
                                prop => prop.Code,
                                new AddressType { Code = code, Name = description, IsActive = active });
                            break;
                        case CLERGY_STATUS_SET:
                            context.ClergyStatuses.AddOrUpdate(
                               p => p.Code,
                               new ClergyStatus { Code = code, Name = description, IsActive = active });
                            break;
                        case CLERGY_TYPE_SET:
                            context.ClergyTypes.AddOrUpdate(
                               p => p.Code,
                               new ClergyType { Code = code, Name = description, IsActive = active });
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
                                new ConstituentType { BaseType = baseType, Code = code, Name = description, IsActive = active, IsRequired = required });
                            break;
                        case CONTACT_TYPE:
                            context.ContactTypes.AddOrUpdate(
                                p => p.Code,
                                new ContactType { Code = code, Description = description, IsActive = active });
                            break;
                        case DENOMINATION_SET:
                            context.Denominations.AddOrUpdate(
                                p => p.Code,
                                new Denomination { Code = code, Name = description, Religion = text1, Affiliation = text2, IsActive = active });
                            break;
                        case EDUCATION_LEVEL_SET:
                            context.EducationLevels.AddOrUpdate(
                                p => p.Code,
                                new EducationLevel { Code = code, Name = description, IsActive = active });
                            break;
                        case ETHNICITY_SET:
                            context.Ethnicities.AddOrUpdate(
                               p => p.Code,
                               new Ethnicity { Code = code, Name = description, IsActive = active });
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
                        case LANGUAGE_SET:
                            context.Languages.AddOrUpdate(
                               p => p.Code,
                               new Language { Code = code, Name = description, IsActive = active });
                            break;
                    }
                }
            }
        }

        private static void LoadAddresses(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {

            string dataFile = "";
            // Load Addresses
            //dataFile = filePath + ADDRESSES_FILE;
            dataFile = Path.Combine(filePath, ADDRESSES_FILE);
            using (var importer = new FileImport(dataFile, "Address"))
            {
                int count = 0;

                while (count <= minCount && importer.GetNextRow())
                {
                    Console.WriteLine(count);
                    count++;
                }
                while (count <= maxCount && importer.GetNextRow())
                {
                    int legacyId = importer.GetInt(0);
                    string streetAddress = importer.GetString(1);
                    string countryCode = importer.GetString(2);
                    string stateCode = importer.GetString(3);
                    int countyFips = importer.GetInt(4);
                    string postalCode = importer.GetString(5);
                    string city = importer.GetString(6);
                    //string region1 = importer.GetString(7);
                    //string region2 = importer.GetString(8);
                    //string region3 = importer.GetString(9);
                    //string region4 = importer.GetString(10);
                    string line2 = "";
                    count++;
                    Console.WriteLine("{0}: {1}", count, legacyId);

                    State state = null;
                    if (stateCode != null && stateCode != "")
                    {
                        //g1 = context.Genders.FirstOrDefault(p => p.Code == gender);
                        try
                        {
                            state = common.States.First(p => p.StateCode == stateCode);
                        }
                        catch (Exception e)
                        { }
                    }

                    Country country = null;
                    if (countryCode == null || countryCode == "")
                    {
                        countryCode = "US";
                    }
                    try
                    {
                        country = common.Countries.First(p => p.CountryCode == countryCode);
                    }
                    catch (Exception e)
                    { }

                    County county = null;
                    if (countyFips != 0)
                    {
                        string fips = countyFips.ToString();
                        while (fips.Length < 5)
                        {
                            fips = "0" + fips;
                        }
                        try
                        {
                            county = common.Counties.First(p => p.FIPSCode == fips);
                        }
                        catch (Exception e)
                        { }
                    }

                    if (state != null && country != null && county != null)
                    {
                        context.Addresses.AddOrUpdate(
                        p => p.LegacyKey,
                        new Address
                        {
                            LegacyKey = legacyId,
                            AddressLine1 = streetAddress,
                            AddressLine2 = line2,
                            City = city,
                            PostalCode = postalCode,
                            StateId = state.Id,
                            CountryId = country.Id,
                            CountyId = county.Id
                        });
                    }
                    else if (state != null && county != null)
                    {
                        context.Addresses.AddOrUpdate(
                        p => p.LegacyKey,
                        new Address
                        {
                            LegacyKey = legacyId,
                            AddressLine1 = streetAddress,
                            AddressLine2 = line2,
                            City = city,
                            PostalCode = postalCode,
                            StateId = state.Id,
                            CountyId = county.Id
                        });
                    }
                    else if (state != null && country != null)
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode,
                                StateId = state.Id,
                                CountryId = country.Id
                            });
                    }
                    else if (county != null && country != null)
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode,
                                CountyId = county.Id,
                                CountryId = country.Id
                            });
                    }
                    else if (state != null)
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode,
                                StateId = state.Id,
                            });
                    }
                    else if (country != null)
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode,
                                CountryId = country.Id
                            });
                    }
                    else if (county != null)
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode,
                                CountyId = county.Id
                            });
                    }
                    else
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress,
                                AddressLine2 = line2,
                                City = city,
                                PostalCode = postalCode
                            });
                    }
                    
                }
            }
        }

        private static void LoadPrefixes(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {
            string dataFile = "";
            // Load prefixes
            //dataFile = filePath + PREFIX_FILE;
            dataFile = Path.Combine(filePath, PREFIX_FILE);
            using (var importer = new FileImport(dataFile, "Prefix"))
            {
                while (importer.GetNextRow())
                {
                    string prefix = importer.GetString(0);
                    string salutation = importer.GetString(1);
                    string label = importer.GetString(2);
                    string gender = importer.GetString(3);
                    string prior = importer.GetString(4);
                    string labelAbbreviation = importer.GetString(5);
                    bool isWebAvailable = (importer.GetString(6) == "yes");
                    Gender g1 = null;

                    if (gender != null && gender != "")
                    {
                        g1 = context.Genders.First(p => p.Code == gender);
                    }
                    if (g1 != null)
                    {
                        context.Prefixes.AddOrUpdate(
                            p => p.Abbreviation,
                            new Prefix { Abbreviation = prefix, Description = label, Gender = g1, GenderId = g1.Id });
                    }
                    else
                    {
                        context.Prefixes.AddOrUpdate(
                            p => p.Abbreviation,
                            new Prefix { Abbreviation = prefix, Description = label });
                    }


                }
            }
        }

        private static void LoadConstituents(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {
            string dataFile = "";
            // Load constituents and constituent denomination cross reference and constituent ethnicity - was 1 to 1 in legacy app
            //dataFile = filePath + INDIVIDUAL_CONSTITUENT_FILE;
            dataFile = Path.Combine(filePath, INDIVIDUAL_CONSTITUENT_FILE);
            using (var importer = new FileImport(dataFile, "Individual"))
            {
                int count = 0;

                while (count <= minCount && importer.GetNextRow())
                {
                    count++;
                    Console.WriteLine(count);
                }

                while (count <= maxCount && importer.GetNextRow())
                {
                    count++;
                    int constituentId = importer.GetInt(0);
                    string constituentType = importer.GetString(1);
                    string name = importer.GetString(2);
                    string name2 = importer.GetString(3);
                    string sourceCode = importer.GetString(4);
                    string taxId = importer.GetString(5);
                    string ethnicity = importer.GetString(6);
                    string denomination = importer.GetString(7);
                    string correspondencePreference = importer.GetString(8);
                    string salutationFormat = importer.GetString(9);
                    string salutationText = importer.GetString(10);
                    string prefix = importer.GetString(11);
                    string firstName = importer.GetString(12);
                    string middleName = importer.GetString(13);
                    string lastName = importer.GetString(14);
                    string suffix = importer.GetString(15);
                    string nickname = importer.GetString(16);
                    string nameFormat = importer.GetString(17);
                    string gender = importer.GetString(18);
                    string earnings = importer.GetString(19);
                    string educationLevel = importer.GetString(20);
                    string employer = importer.GetString(21);
                    string position = importer.GetString(22);
                    string empStartDate = importer.GetString(23);
                    //DateTime employmentStartDate;
                    //if (empStartDate != "?")
                    //{
                    //    DateTime.TryParse(empStartDate, out employmentStartDate);
                    //}
                    string empEndDate = importer.GetString(24);
                    //DateTime employmentEndDate;
                    //if (empEndDate != "?")
                    //{
                    //    DateTime.TryParse(empEndDate, out employmentEndDate);
                    //}
                    string firstEmpDate = importer.GetString(25);
                    //DateTime firstEmploymentDate;
                    //if (firstEmpDate != "?")
                    //{
                    //    DateTime.TryParse(firstEmpDate, out firstEmploymentDate);
                    //}
                    bool isClientEmployee = (importer.GetString(26) == "yes");
                    string profession = importer.GetString(27);
                    string clergyType = importer.GetString(28);
                    string clergyStatus = importer.GetString(29);
                    string ordDate = importer.GetString(30);
                    //DateTime ordinationDate;
                    //if (ordDate != "?")
                    //{
                    //    DateTime.TryParse(ordDate, out ordinationDate);
                    //}
                    string ordinationPlace = importer.GetString(31);
                    string prosDate = importer.GetString(32);
                    //DateTime prospectDate;
                    //if (prosDate != "?")
                    //{
                    //    DateTime.TryParse(prosDate, out prospectDate);
                    //}
                    int maritalStatus = importer.GetInt(33);
                    string marDate = importer.GetString(34);
                    //DateTime marriageDate;
                    //if (marDate != "?")
                    //{
                    //    DateTime.TryParse(marDate, out marriageDate);
                    //}
                    string divDate = importer.GetString(35);
                    //DateTime divorceDate;
                    //if (divDate != "?")
                    //{
                    //    DateTime.TryParse(divDate, out divorceDate);
                    //}
                    string decDate = importer.GetString(36);
                    //DateTime deceasedDate;
                    //if (decDate != "?")
                    //{
                    //    DateTime.TryParse(decDate, out deceasedDate);
                    //}
                    int birthMonth = importer.GetInt(37);
                    int birthDay = importer.GetInt(38);
                    int birthYear1 = importer.GetInt(39);
                    int birthYear2 = importer.GetInt(40);
                    string deletionCode = importer.GetString(41);
                    string delDate = importer.GetString(42);
                    //DateTime deletionDate;
                    //if (delDate != "?")
                    //{
                    //    DateTime.TryParse(importer.GetString(42), out deletionDate);
                    //}

                    //Gender g1;
                    //if (gender != null && gender != "")
                    //{
                    //    try
                    //    {
                    //        g1 = context.Genders.FirstOrDefault(p => p.Code == gender);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //ClergyStatus cs1 = null;
                    //if (clergyStatus != null)
                    //{
                    //    try
                    //    {
                    //        cs1 = context.ClergyStatuses.First(p => p.Code == clergyStatus);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //ClergyType ct1;
                    //if (clergyType != null)
                    //{
                    //    try
                    //    {
                    //        ct1 = context.ClergyTypes.First(p => p.Code == clergyType);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //ConstituentStatus cs2;
                    //if (deletionDate == null)
                    //{
                    //    cs2 = context.ConstituentStatuses.First(p => p.Code == "AC");
                    //}
                    //else
                    //{
                    //    cs2 = context.ConstituentStatuses.First(p => p.Code == "IN");
                    //}

                    //ConstituentType ct2;
                    //if (constituentType != null)
                    //{
                    //    try
                    //    {
                    //        ct2 = context.ConstituentTypes.First(p => p.Code == constituentType);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //EducationLevel e1;
                    //if (educationLevel != null)
                    //{
                    //    try
                    //    {
                    //        e1 = context.EducationLevels.First(p => p.Code == educationLevel);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    string formattedName = firstName;
                    if (middleName != null)
                    {
                        formattedName = formattedName + " " + middleName;
                    }
                    if (lastName != null)
                    {
                        formattedName = formattedName + " " + lastName;
                    }

                    //DateTime birthDate;
                    //if (birthMonth != 0 && birthDay != 0 && birthYear1 != 0)
                    //{
                    //    birthDate = new DateTime(birthYear1, birthMonth, birthDay);
                    //}
                    //else
                    //{
                    //    birthDate = new DateTime(1000, 1, 1);
                    //}

                    //IncomeLevel il1;
                    //if (earnings != null)
                    //{
                    //    try
                    //    {
                    //        il1 = context.IncomeLevels.First(p => p.Code == earnings);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //Prefix p1;
                    //if (prefix != null)
                    //{
                    //    try
                    //    {
                    //        p1 = context.Prefixes.First(p => p.Abbreviation == prefix);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //Profession p2;
                    //if (profession != null)
                    //{
                    //    try
                    //    {
                    //        p2 = context.Professions.First(p => p.Code == profession);
                    //    }
                    //    catch (Exception e)
                    //    { }
                    //}

                    //TODO: run salutation through the salutation logic when it gets there if salutation text is not filled in.
                    //Console.WriteLine(birthYear1);
                    //Console.WriteLine(birthYear2);
                    //Console.WriteLine(cs2.Id);
                    Console.WriteLine("{0}: {1}", count, constituentId);
                    //Console.WriteLine(ct2.Id);
                    //Console.WriteLine(deceasedDate);
                    //Console.WriteLine(divorceDate);
                    //Console.WriteLine(employer);
                    //Console.WriteLine(employmentStartDate);
                    //Console.WriteLine(employmentEndDate);
                    //Console.WriteLine(firstEmploymentDate);
                    //Console.WriteLine(firstName);
                    //Console.WriteLine(formattedName);
                    //Console.WriteLine(isClientEmployee);
                    //Console.WriteLine(lastName);
                    //Console.WriteLine(marriageDate);
                    //Console.WriteLine(middleName);
                    //Console.WriteLine(name2);
                    //Console.WriteLine(nickname);
                    //Console.WriteLine(ordinationDate);
                    //Console.WriteLine(ordinationPlace);
                    //Console.WriteLine(position);
                    //Console.WriteLine(prospectDate);
                    //Console.WriteLine(salutationText);
                    //Console.WriteLine(sourceCode);
                    //Console.WriteLine(suffix);
                    //Console.WriteLine(taxId);

                    ////TODO: create constituent
                    //Constituent constituent = null;
                    //try
                    //{
                    //    constituent = context.Constituents.First<Constituent>(p => p.ConstituentNumber == constituentId);
                    //} catch (Exception)
                    //{ }
                    //if (constituent == null)
                    //{
                    //    //constituent = context.Constituents.Create<Constituent>();
                    //    constituent = context.Constituents.Create();
                    //    constituent.ConstituentNumber = constituentId;
                    //}
                    //constituent.BirthDay = birthDay;
                    //constituent.BirthMonth = birthMonth;
                    //constituent.BirthYearFrom = birthYear1;
                    //constituent.BirthYearTo = birthYear2;
                    ////constituent.BirthDateType = ?;
                    //if (cs1 != null)
                    //{
                    //    //constituent.ClergyStatus = cs1;
                    //    constituent.ClergyStatusId = cs1.Id;
                    //}
                    ////constituent.ClergyType = ;
                    ////constituent.ClergyTypeId = ;
                    ////constituent.ConstituentType = ;
                    ////constituent.ConstituentTypeId = ;
                    ////constituent.CorrespondencePreference = ;
                    ////constituent.DeceasedDate = ;
                    ////constituent.Denominations = ;
                    ////constituent.DivorceDate = ;
                    ////constituent.EducationLevel = ;
                    ////constituent.EducationLevelId = ;
                    ////constituent.Employer = ;
                    ////constituent.EmploymentEndDate = ;
                    ////constituent.EmploymentStartDate = ;
                    ////constituent.FirstEmploymentDate = ;
                    ////constituent.FirstName = ;
                    ////constituent.FormattedName = ;
                    ////constituent.Gender = ;
                    ////constituent.GenderId = ;
                    ////constituent.IncomeLevel = ;
                    ////constituent.IncomeLevelId = ;
                    ////constituent.IsEmployee = ;
                    ////constituent.IsIRSLetterReceived =;
                    ////constituent.IsTaxExempt = ;
                    ////constituent.Language = ;
                    ////constituent.LanguageId = ;
                    ////constituent.LastName = ;
                    ////constituent.MaritalStatus = ;
                    ////constituent.MarriageDate = ;
                    ////constituent.MembershipCount = ;
                    ////constituent.MiddleName = ;
                    ////constituent.Name = ;
                    ////constituent.Name2 = ;
                    ////constituent.NameFormat = ;
                    ////constituent.Nickname = ;
                    ////constituent.OrdinationDate = ;
                    ////constituent.PlaceOfOrdination = ;
                    ////constituent.Position = ;
                    ////constituent.Prefix = ;
                    ////constituent.PrefixId = ;
                    ////constituent.Profession = ;
                    ////constituent.ProfessionId = ;
                    ////constituent.ProspectDate = ;
                    ////constituent.Salutation = ;
                    ////constituent.SalutationType = ;
                    ////constituent.Source = ;
                    ////constituent.Suffix = ;
                    ////constituent.TaxExemptVerifyDate = ;
                    ////constituent.TaxId = ;
                    ////constituent.YearEstablished = ;
                    ////context.SaveChanges();
                    //context.SaveChangesAsync();

                    context.Constituents.AddOrUpdate(
                        p => p.ConstituentNumber,
                        new Constituent
                        {
                            //BirthDate = birthDate,
                            BirthDay = birthDay,
                            BirthMonth = birthMonth,
                            BirthYearFrom = birthYear1,
                            BirthYearTo = birthYear2,
                            Business = profession,
                            //ClergyStatus = cs1,
                            //ClergyStatusId = cs1.Id,
                            //ClergyType = ct1,
                            //ClergyTypeId = ct1.Id,
                            //ConstituentStatus = cs2,
                            //ConstituentStatusId = cs2.Id,
                            ConstituentNumber = constituentId,
                            //ConstituentType = ct2,
                            //ConstituentTypeId = ct2.Id,
                            //DeceasedDate = deceasedDate,
                            //DivorceDate = divorceDate,
                            //EducationLevel = e1,
                            Employer = employer,
                            //EmploymentStartDate = employmentStartDate,
                            //EmploymentEndDate = employmentEndDate,
                            //FirstEmploymentDate = firstEmploymentDate,
                            FirstName = firstName,
                            FormattedName = formattedName,
                            //IncomeLevel = il1,
                            //IncomeLevelId = il1.Id,
                            IsEmployee = isClientEmployee,
                            LastName = lastName,
                            MaritalStatus = maritalStatus,
                            //MarriageDate = marriageDate,
                            MiddleName = middleName,
                            Name2 = name2,
                            Nickname = nickname,
                            //OrdinationDate = ordinationDate,
                            PlaceOfOrdination = ordinationPlace,
                            Position = position,
                            //Prefix = p1,
                            //PrefixId = p1.Id,
                            //Profession = p2,
                            //ProfessionId = p2.Id,
                            //ProspectDate = prospectDate,
                            Salutation = salutationText,
                            Source = sourceCode,
                            Suffix = suffix,
                            TaxId = taxId
                        });

                    //Constituent constituent =  context.Constituents.First<Constituent>(p => p.ConstituentNumber == constituentId);


                }
            }
        }

        private static void LoadConstituentAddress(DomainContext context, CommonContext common, string organization, string filePath, int minCount, int maxCount)
        {

            string dataFile = "";
            dataFile = Path.Combine(filePath, CONSTITUENT_ADDRESS_FILE);

            using (var importer = new FileImport(dataFile, "ConstituentAddress"))
            {
                int count = 0;

                while (count <= minCount && importer.GetNextRow())
                {
                    count++;
                    Console.WriteLine(count);
                }

                while (count <= maxCount && importer.GetNextRow())
                {
                    count++;

                    int legacyAddressId = importer.GetInt(0);
                    int constituentNum = importer.GetInt(1);

                    //string addressTypeCode = importer.GetString(2);
                    //string comment = importer.GetString(3);
                    //DateTime startDate;
                    //DateTime.TryParse(importer.GetString(4), out startDate);
                    //DateTime endDate;
                    //DateTime.TryParse(importer.GetString(5), out endDate);
                    //int startDay;
                    //int.TryParse(importer.GetString(6), out startDay);
                    //int endDay;
                    //int.TryParse(importer.GetString(7), out endDay);
                    bool isPrimary = importer.GetBool(8);

                    //string residentType = importer.GetString(9);
                    importer.LogMessage($"{count}: {legacyAddressId} {constituentNum}");

                    Address a1 = null;
                    Constituent c1 = null;
                    if (legacyAddressId != 0)
                    {
                        try
                        {
                            a1 = context.Addresses.First(p => p.LegacyKey == legacyAddressId);
                        }
                        catch (Exception e)
                        { }
                    }
                    if (constituentNum != 0)
                    {
                        try
                        {
                            c1 = context.Constituents?.First(p => p.ConstituentNumber == constituentNum);
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
        }

        //public static string[) ParseLine(string line)
        //{
        //    string[) parts = line.Split(',');

        //    int i = 0;
        //    while (i < parts.Length - 1)
        //    {
        //        if (importer.GetString(i) != null && importer.GetString(i) != "")
        //        {
        //            if (importer.GetString(i).First() == '"' && importer.GetString(i).Last() != '"')
        //            {
        //                importer.GetString(i) = importer.GetString(i) + "," + importer.GetString(i + 1);

        //                int j = i+1;
        //                while (j < parts.Length - 1)
        //                {
        //                    importer.GetString(j) = importer.GetString(j+1);
        //                    j++;
        //                }
        //            }
        //        }
        //        i++;

        //    }

        //    return parts;
        //}
    }
}
