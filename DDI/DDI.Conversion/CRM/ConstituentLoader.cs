using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Conversion;
using DDI.Data;
using DDI.Data.Enums.CRM;
using DDI.Data.Models.Client.Core;
using DDI.Data.Models.Client.CRM;
using DDI.Data.Models.Common;

namespace DDI.Conversion.CRM
{
    internal class ConstituentLoader : IDataConversion
    {


        private ConversionArgs _args;
        private string _crmDirectory;

        public void Execute(ConversionArgs args)
        {
            _args = args;
            _crmDirectory = Path.Combine(_args.BaseDirectory, "CRM");

            LoadIndividuals("Individual.csv");

            //LoadAddresses("Address.csv");
            //LoadConstituents("Individual.csv");
            //LoadConstituentAddress("ConstituentAddress.csv");

            ///LoadDoingBusinessAs();
            //LoadEducation();
            //LoadPaymentPreferences();
            //LoadAleternateIDs();
            //LoadContactInfo();

        }




        private void LoadAddresses(string filename)
        {
            DomainContext context = new DomainContext();
            var common = new CommonContext();
            string dataFile = "";
            dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "Address"))
            {
                int count = 0;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    int legacyId = importer.GetInt(0);
                    string streetAddress1 = importer.GetString(1);
                    string streetAddress2 = importer.GetString(2);
                    string countryCode = importer.GetString(3);
                    string stateCode = importer.GetString(4);
                    int countyFips = importer.GetInt(5);
                    string postalCode = importer.GetString(6);
                    string city = importer.GetString(7);
                    string region1 = importer.GetString(8);
                    string region2 = importer.GetString(9);
                    string region3 = importer.GetString(10);
                    string region4 = importer.GetString(11);
                    
                    count++;
                    Console.WriteLine("{0}: {1}", count, legacyId);

                    State state = null;
                    if (!string.IsNullOrWhiteSpace(stateCode))
                    {
                        //g1 = context.Genders.FirstOrDefault(p => p.Code == gender);
                        try
                        {
                            state = common.States.First(p => p.StateCode == stateCode);
                        }
                        catch (Exception e)
                        {}
                    }

                    Country country = null;
                    if (string.IsNullOrWhiteSpace(countryCode))
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

                    Region regionOne = null;
                    if (!string.IsNullOrWhiteSpace(region1))
                    {
                        try
                        {
                            regionOne = context.Regions.First(p => p.Level == 1 && p.Code == region1);

                        }
                        catch (Exception e)
                        { }
                    }

                    Region regionTwo = null;
                    if (!string.IsNullOrWhiteSpace(region2))
                    {
                        try
                        {
                            regionTwo = context.Regions.First(p => p.Level == 2 && p.Code == region2);

                        }
                        catch (Exception e)
                        { }
                    }

                    Region regionThree = null;
                    if (!string.IsNullOrWhiteSpace(region3))
                    {
                        try
                        {
                            regionThree = context.Regions.First(p => p.Level == 3 && p.Code == region3);

                        }
                        catch (Exception e)
                        { }
                    }

                    Region regionFour = null;
                    if (!string.IsNullOrWhiteSpace(region4))
                    {
                        try
                        {
                            regionFour = context.Regions.First(p => p.Level == 4 && p.Code == region4);

                        }
                        catch (Exception e)
                        { }
                    }

                    if (state != null && county != null && !string.IsNullOrWhiteSpace(streetAddress1))
                    {
                        context.Addresses.AddOrUpdate(
                            p => p.LegacyKey,
                            new Address
                            {
                                LegacyKey = legacyId,
                                AddressLine1 = streetAddress1,
                                AddressLine2 = streetAddress2,
                                City = city,
                                PostalCode = postalCode,
                                State = state,
                                Country = country,
                                County = county,
                                Region1 = regionOne,
                                Region2 = regionTwo,
                                Region3 = regionThree,
                                Region4 = regionFour
                            });
                    }

                    if (count % 1000 == 0)
                    {
                        context.SaveChanges();
                        context.Dispose();
                        context = new DomainContext();
                    }
                }
            }

            context.SaveChanges();
            
        }

        private DomainContext CreateContextForConstituents()
        {
            DomainContext context = new DomainContext();

            // Load entity sets that will be queried often...
            context.ConstituentTypes.Load();
            context.Ethnicities.Load();
            context.Denominations.Load();
            context.Prefixes.Load();
            context.Genders.Load();
            context.IncomeLevels.Load();
            context.EducationLevels.Load();
            context.Professions.Load();
            context.ClergyTypes.Load();
            context.ClergyStatuses.Load();
            context.MaritalStatuses.Load();
            context.ConstituentStatuses.Load();

            return context;
        }

        private void LoadIndividuals(string filename)
        {
            DomainContext context = CreateContextForConstituents();

            string dataFile = "";
            dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "Individual"))
            {
                int count = 0;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    count++;
                    int constituentNum = importer.GetInt(0);

                    if (constituentNum == 0)
                    {
                        continue;
                    }

                    string constituentTypeCode = importer.GetString(1);
                    string name = importer.GetString(2);
                    string name2 = importer.GetString(3);
                    string sourceCode = importer.GetString(4);
                    string taxId = importer.GetString(5);
                    string ethnicity = importer.GetString(6);
                    string denomination = importer.GetString(7);
                    string correspondencePreference = importer.GetString(8);
                    string salutationFormat = importer.GetString(9);
                    string salutationText = importer.GetString(10);
                    string prefixCode = importer.GetString(11);
                    string firstName = importer.GetString(12);
                    string middleName = importer.GetString(13);
                    string lastName = importer.GetString(14);
                    string suffix = importer.GetString(15);
                    string nickname = importer.GetString(16);
                    string nameFormat = importer.GetString(17);
                    string genderCode = importer.GetString(18);
                    string earningsCode = importer.GetString(19);
                    string educationLevelCode = importer.GetString(20);
                    string employer = importer.GetString(21);
                    string position = importer.GetString(22);
                    DateTime? employmentStartDate = importer.GetDateTime(23);
                    DateTime? employmentEndDate = importer.GetDateTime (24);
                    DateTime? firstEmploymentDate = importer.GetDateTime(25);
                    bool isClientEmployee = (importer.GetString(26) == "yes");
                    string professionCode = importer.GetString(27);
                    string clergyTypeCode = importer.GetString(28);
                    string clergyStatusCode = importer.GetString(29);
                    DateTime? ordinationDate = importer.GetDateTime(30);
                    string ordinationPlace = importer.GetString(31);
                    DateTime? prospectDate = importer.GetDateTime(32);
                    string maritalStatusCode = importer.GetString(33);
                    DateTime? marriageDate = importer.GetDateTime(34);
                    DateTime? divorceDate = importer.GetDateTime(35);
                    DateTime? deceasedDate = importer.GetDateTime(36);
                    int birthMonth = importer.GetInt(37);
                    int birthDay = importer.GetInt(38);
                    int birthYear1 = importer.GetInt(39);
                    int birthYear2 = importer.GetInt(40);
                    string deletionCode = importer.GetString(41);
                    DateTime? deleteDate = importer.GetDateTime(42);

                    ConstituentType constituentType = context.ConstituentTypes.Local.FirstOrDefault(p => p.Code == constituentTypeCode);
                    if (constituentType == null)
                    {
                        importer.LogError($"PIN {constituentNum} has invalid constituent type \"{constituentType}\".");
                        continue;
                    }

                    
                    Constituent constituent = context.Constituents
                                                     .Include(p => p.Ethnicities)
                                                     .Include(p => p.Denominations)
                                                     .FirstOrDefault(p => p.ConstituentNumber == constituentNum);
                    if (constituent == null)
                    {
                        constituent = new Constituent();
                        constituent.ConstituentNumber = constituentNum;
                        context.Constituents.Add(constituent);
                    }

                    constituent.ConstituentTypeId = constituentType.Id;
                    constituent.Name = name;
                    constituent.Name2 = name2;
                    constituent.Source = sourceCode;
                    constituent.TaxId = taxId;
                    constituent.Salutation = salutationText;
                    constituent.FirstName = firstName;
                    constituent.MiddleName = middleName;
                    constituent.LastName = lastName;
                    constituent.Suffix = suffix;
                    constituent.Nickname = nickname;
                    constituent.NameFormat = nameFormat;
                    constituent.Employer = employer;
                    constituent.Position = position;
                    constituent.EmploymentStartDate = employmentStartDate;
                    constituent.EmploymentEndDate = employmentEndDate;
                    constituent.FirstEmploymentDate = firstEmploymentDate;
                    constituent.IsEmployee = isClientEmployee;
                    constituent.OrdinationDate = ordinationDate;
                    constituent.PlaceOfOrdination = ordinationPlace;
                    constituent.ProspectDate = prospectDate;
                    constituent.MarriageDate = marriageDate;
                    constituent.DivorceDate = divorceDate;
                    constituent.DeceasedDate = deceasedDate;
                    constituent.BirthDay = birthDay;
                    constituent.BirthMonth = birthMonth;
                    constituent.BirthYearFrom = birthYear1;
                    constituent.BirthYearTo = birthYear2;

                    // Ethnicity
                    string[] codelist = ethnicity.Split(',');
                    if (constituent.Ethnicities == null)
                    {
                        constituent.Ethnicities = new List<Ethnicity>();
                    }
                    foreach (Ethnicity ethnicityToRemove in constituent.Ethnicities.ToList())
                    {
                        if (!codelist.Any(p => p == ethnicityToRemove.Code))
                        {
                            constituent.Ethnicities.Remove(ethnicityToRemove);
                        }
                    }
                    foreach (string code in codelist)
                    {
                        Ethnicity ethnicityToAdd = context.Ethnicities.Local.FirstOrDefault(p => p.Code == code);
                        if (ethnicityToAdd == null)
                        {
                            importer.LogError($"Invalid ethnicity code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.Ethnicities.Add(ethnicityToAdd);
                        }
                    }

                    // Denominations
                    codelist = denomination.Split(',');
                    if (constituent.Denominations == null)
                    {
                        constituent.Denominations = new List<Denomination>();
                    }
                    foreach (Denomination denominationToRemove in constituent.Denominations.ToList())
                    {
                        if (!codelist.Any(p => p == denominationToRemove.Code))
                        {
                            constituent.Denominations.Remove(denominationToRemove);
                        }
                    }
                    foreach (string code in codelist)
                    {
                        Denomination denominationToAdd = context.Denominations.Local.FirstOrDefault(p => p.Code == code);
                        if (denominationToAdd == null)
                        {
                            importer.LogError($"Invalid denomination code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.Denominations.Add(denominationToAdd);
                        }
                    }

                    // Correspondence preference
                    switch(correspondencePreference)
                    {
                        case "E": constituent.CorrespondencePreference = CorrespondencePreference.Email; break;
                        case "EP": constituent.CorrespondencePreference = CorrespondencePreference.Both; break;
                        case "P": constituent.CorrespondencePreference = CorrespondencePreference.Paper; break;
                        default: constituent.CorrespondencePreference = CorrespondencePreference.None; break;
                    }

                    // Salutation format
                    switch (salutationFormat)
                    {
                        case "0": constituent.SalutationType = SalutationType.Default; break;
                        case "1": constituent.SalutationType = SalutationType.Formal; break;
                        case "2": constituent.SalutationType = SalutationType.Informal; break;
                        case "3": constituent.SalutationType = SalutationType.FormalSeparate; break;
                        case "4": constituent.SalutationType = SalutationType.InformalSeparate; break;
                        default: constituent.SalutationType = SalutationType.Custom; break;
                    }

                    // Prefix
                    if (string.IsNullOrWhiteSpace(prefixCode))
                    {
                        constituent.PrefixId = null;
                    }
                    else
                    {
                        Prefix prefix = context.Prefixes.Local.FirstOrDefault(p => p.Code == prefixCode);
                        if (prefix == null)
                        {
                            importer.LogError($"Invalid prefix code {prefixCode} for PIN {constituentNum}.");
                            constituent.PrefixId = null;
                        }
                        else
                        {
                            constituent.PrefixId = prefix.Id;
                        }
                    }

                    // Gender
                    if (string.IsNullOrWhiteSpace(genderCode))
                    {
                        constituent.GenderId = null;
                    }
                    else
                    {
                        Gender gender = context.Genders.Local.FirstOrDefault(p => p.Code == genderCode);
                        if (gender == null)
                        {
                            importer.LogError($"Invalid gender code {genderCode} for PIN {constituentNum}.");
                            constituent.GenderId = null;
                        }
                        else
                        {
                            constituent.GenderId = gender.Id;
                        }
                    }

                    // Earnings
                    if (string.IsNullOrWhiteSpace(earningsCode))
                    {
                        constituent.IncomeLevelId = null;
                    }
                    else
                    {
                        IncomeLevel incomeLevel = context.IncomeLevels.Local.FirstOrDefault(p => p.Code == earningsCode);
                        if (incomeLevel == null)
                        {
                            importer.LogError($"Invalid earnings code {earningsCode} for PIN {constituentNum}.");
                            constituent.IncomeLevelId = null;
                        }
                        else
                        {
                            constituent.IncomeLevelId = incomeLevel.Id;
                        }
                    }

                    // Education Level
                    if (string.IsNullOrWhiteSpace(educationLevelCode))
                    {
                        constituent.EducationLevelId = null;
                    }
                    else
                    {
                        EducationLevel educationLevel = context.EducationLevels.Local.FirstOrDefault(p => p.Code == educationLevelCode);
                        if (educationLevel == null)
                        {
                            importer.LogError($"Invalid education level code {educationLevelCode} for PIN {constituentNum}.");
                            constituent.EducationLevelId = null;
                        }
                        else
                        {
                            constituent.EducationLevelId = educationLevel.Id;
                        }
                    }

                    // Profession
                    if (string.IsNullOrWhiteSpace(professionCode))
                    {
                        constituent.ProfessionId = null;
                    }
                    else
                    {
                        Profession profession = context.Professions.Local.FirstOrDefault(p => p.Code == professionCode);
                        if (profession == null)
                        {
                            importer.LogError($"Invalid profession code {professionCode} for PIN {constituentNum}.");
                            constituent.ProfessionId = null;
                        }
                        else
                        {
                            constituent.ProfessionId = profession.Id;
                        }
                    }

                    // Clergy Type
                    if (string.IsNullOrWhiteSpace(clergyTypeCode))
                    {
                        constituent.ClergyTypeId = null;
                    }
                    else
                    {
                        ClergyType clergyType = context.ClergyTypes.Local.FirstOrDefault(p => p.Code == clergyTypeCode);
                        if (clergyType == null)
                        {
                            importer.LogError($"Invalid clergy type code {clergyTypeCode} for PIN {constituentNum}.");
                            constituent.ClergyTypeId = null;
                        }
                        else
                        {
                            constituent.ClergyTypeId = clergyType.Id;
                        }
                    }

                    // Clergy Status
                    if (string.IsNullOrWhiteSpace(clergyStatusCode))
                    {
                        constituent.ClergyStatusId = null;
                    }
                    else
                    {
                        ClergyStatus clergyStatus = context.ClergyStatuses.Local.FirstOrDefault(p => p.Code == clergyStatusCode);
                        if (clergyStatus == null)
                        {
                            importer.LogError($"Invalid clergy status code {clergyStatusCode} for PIN {constituentNum}.");
                            constituent.ClergyStatusId = null;
                        }
                        else
                        {
                            constituent.ClergyStatusId = clergyStatus.Id;
                        }
                    }

                    // Constituent Status

                    if (string.IsNullOrWhiteSpace(deletionCode))
                    {
                        constituent.ConstituentStatusId = null;
                    }
                    else
                    {
                        ConstituentStatus constituentStatus = context.ConstituentStatuses.Local.FirstOrDefault(p => p.Code == deletionCode);
                        if (constituentStatus == null)
                        {
                            importer.LogError($"Invalid constituent status code {deletionCode} for PIN {constituentNum}.");
                            constituent.ConstituentStatusId = null;
                        }
                        else
                        {
                            constituent.ConstituentStatusId = constituentStatus.Id;
                        }
                    }

                    // Anything coming over with a deletion date should be set to status deleted.
                    if (deleteDate.HasValue)
                    {
                        if (constituent.ConstituentStatusId == null)
                        {
                            constituent.ConstituentStatusId = context.ConstituentStatuses.Local.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_DELETED).Id;
                        }
                    }
                    else
                    {
                        if (constituent.ConstituentStatusId == null)
                        {
                            // If no status, set to active.
                            constituent.ConstituentStatusId = context.ConstituentStatuses.Local.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_ACTIVE).Id;
                        }
                        else
                        {
                            // TODO: Set constituent status date to 1/1/1990.  (DC-266)
                        }
                    }
                    // TODO: Constituent status date missing (DC-266).

                    // Marital Status - Cannot be done yet because model is incorrect. (DC-173)

                    
                    if (birthDay > 0 && birthMonth > 0 && birthYear1> 0)
                    {
                        constituent.BirthDateType = BirthDateType.FullDate;
                    }
                    else if (birthDay > 0 && birthMonth > 0)
                    {
                        constituent.BirthDateType = BirthDateType.MonthDay;
                    }
                    else if (birthYear1 != 0 && birthYear2 != 0)
                    {
                        constituent.BirthDateType = BirthDateType.AgeRange;
                    }
                    else
                    {
                        constituent.BirthDateType = BirthDateType.None;
                    }

                    if (count % 100000 == 0)
                    {
                        context.SaveChanges();
                        context.Dispose();
                        context = CreateContextForConstituents();
                    }
                }
            }

            context.SaveChanges();
        }

        private void LoadConstituentAddress(string filename)
        {
            DomainContext context = new DomainContext();
            string dataFile = "";
            dataFile = Path.Combine(_crmDirectory, filename);

            using (var importer = new FileImport(dataFile, "ConstituentAddress"))
            {
                int count = 0;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    count++;

                    int legacyAddressId = importer.GetInt(0);
                    int constituentNum = importer.GetInt(1);

                    //TODO: clean this up and make it functional. model in constituent and prefix seem to work better
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

                    if (count % 1000 == 0)
                    {
                        context.SaveChanges();
                        context.Dispose();
                        context = new DomainContext();
                    }
                }
            }
            context.SaveChanges();
        }


    }
}
