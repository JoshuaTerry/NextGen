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
    internal class LoadDataCRM : IDataConversion
    {
        private const string ORGANIZATION_CONSTITUENT_FILE = "organization.csv";
        private const string DOING_BUSINESS_AS_FILE = "constituentDBA.csv";
        private const string EDUCATION_FILE = "EducationInfo.csv";
        private const string ALTERNATE_ID_FILE = "AlternateID.csv";
        private const string CONTACT_INFO_FILE = "ContactInfo.csv";

        // nacodes.record-cd sets - these are the ones that are being imported here.
        private const int DENOMINATION_SET = 5;
        private const int ADDRESS_TYPE_SET = 6;
        private const int CONSTITUENT_TYPE = 7;
        private const int EDUCATION_LEVEL_SET = 8;
        private const int STATE_SET = 10;
        private const int PROFESSION_SET = 11;
        private const int EARNINGS_SET = 14;
        private const int DELETION_SET = 15;
        private const int LANGUAGE_SET = 26;
        private const int ETHNICITY_SET = 33;
        private const int CLERGY_TYPE_SET = 34;
        private const int CLERGY_STATUS_SET = 35;
        private const int GENDER_SET = 38;
        private const int MARITAL_STATUS_SET = 39;
        private const int SCHOOL_SET = 41;
        private const int DEGREE_SET = 42;
        private const int CONTACT_CATEGORY = 75;

        private ConversionArgs _args;
        private string _crmDirectory;

        public void Execute(ConversionArgs args)
        {
            _args = args;
            _crmDirectory = Path.Combine(_args.BaseDirectory, "CRM");

            //LoadLegacyCodes("NACodes.csv");
            //LoadContactTypes("ContactType.csv");
            //LoadPrefixes("NamePrefix.csv");
            //LoadRegionLevels("RegionLevel.csv");
            //LoadRegions("Region.csv");
            LoadRegionAreas("RegionAreas.csv");

            //LoadAddresses("Address.csv");
            //LoadConstituents("Individual.csv");
            //LoadConstituentAddress("ConstituentAddress.csv");

            ///LoadDoingBusinessAs();
            //LoadEducation();
            //LoadPaymentPreferences();
            //LoadAleternateIDs();
            //LoadContactInfo();

        }


        private void LoadLegacyCodes(string filename)
        {
            DomainContext context = new DomainContext();
            string dataFile = Path.Combine(_crmDirectory, filename);

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
                        case DENOMINATION_SET:
                            Affiliation affiliation;
                            Religion religion;

                            switch (text1.ToUpper())
                            {
                                case "PROT": religion = Religion.Protestant; break;
                                case "ORTH": religion = Religion.Orthodox; break;
                                case "BUD": religion = Religion.Buddhist; break;
                                case "HIND": religion = Religion.Hindu; break;
                                case "ISL": religion = Religion.Islam; break;
                                case "JEW": religion = Religion.Jewish; break;
                                case "CATH": religion = Religion.Catholic; break;
                                default: religion = Religion.None; break;
                            }

                            switch (text2.ToUpper())
                            {
                                case "AC":
                                case "A":
                                    affiliation = Affiliation.Affiliated; break;
                                case "UC":
                                case "U":
                                    affiliation = Affiliation.Unaffiliated; break;
                                case "OC":
                                case "O":
                                    affiliation = Affiliation.Other; break;
                                default:
                                    affiliation = Affiliation.None; break;
                            }

                            context.Denominations.AddOrUpdate(
                                p => p.Code,
                                new Denomination { Code = code, Name = description, IsActive = active, Religion = religion, Affiliation = affiliation });
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
                            }
                            else if (code == "F")
                            {
                                masculine = false;
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
                        case DELETION_SET:
                            if (code == "AC" || code == "BL" || code == "HO" || code == "DEL")
                            {
                                continue;
                            }
                            context.ConstituentStatuses.AddOrUpdate(
                                p => p.Code,
                                new ConstituentStatus() { Code = code, Name = description, BaseStatus = ConstituentBaseStatus.Inactive, IsActive = active, IsRequired = false });
                            break;
                        case PROFESSION_SET:
                            context.Professions.AddOrUpdate(
                               p => p.Code,
                               new Profession { Code = code, Name = description, IsActive = active });
                            break;
                        case EARNINGS_SET:
                            context.IncomeLevels.AddOrUpdate(
                               p => p.Code,
                               new IncomeLevel { Code = code, Name = description, IsActive = active });
                            break;
                        case MARITAL_STATUS_SET:
                            context.MaritalStatuses.AddOrUpdate(
                               p => p.Code,
                               new MaritalStatus { Code = code, Name = description, IsActive = active });
                            break;
                        case SCHOOL_SET:
                            context.Schools.AddOrUpdate(
                               p => p.Code,
                               new School { Code = code, Name = description, IsActive = active });
                            break;
                        case DEGREE_SET:
                            context.Degrees.AddOrUpdate(
                               p => p.Code,
                               new Degree { Code = code, Name = description, IsActive = active });
                            break;

                    }
                }
            }
            context.SaveChanges();
        }

        private void LoadContactTypes(string filename)
        {
            DomainContext context = new DomainContext();
            string dataFile = Path.Combine(_crmDirectory, filename);

            using (var importer = new FileImport(dataFile, "ContactType"))
            {
                int count = 1;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    string categoryCode = importer.GetString(0);
                    string typeCode = importer.GetString(1);

                    if (string.IsNullOrWhiteSpace(typeCode))
                    {
                        continue;
                    }

                    ContactCategory category = context.ContactCategories.FirstOrDefault(p => p.Code == categoryCode);

                    bool isDefault = (categoryCode == ContactCategory.EMAIL && typeCode == "H") ||
                                (categoryCode == ContactCategory.WEB && typeCode == "H") ||
                                (categoryCode == ContactCategory.PHONE && typeCode == "H") ||
                                (categoryCode == ContactCategory.PERSON && typeCode == "C") ||
                                (categoryCode == ContactCategory.SOCIAL && typeCode == "F");

                    var contactType = new ContactType()
                    {
                        ContactCategory = category,
                        Code = typeCode,
                        Name = importer.GetString(2),
                        IsAlwaysShown = importer.GetBool(4),
                        CanDelete = importer.GetBool(6),
                        IsActive = importer.GetBool(5)
                    };

                    context.ContactTypes.AddOrUpdate(p => p.Code, contactType);

                    if (isDefault)
                    {
                        category.DefaultContactType = contactType;
                    }

                    count++;
                }
                context.SaveChanges();
            }
        }

        private void LoadRegionLevels(string filename)
        {
            DomainContext context = new DomainContext();
            string dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "RegionLevel"))
            {
                int count = 1;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    string levelText = importer.GetString(0);
                    string regionLabel = importer.GetString(1);
                    string regionAbbreviation = importer.GetString(2);
                    bool isRequired = importer.GetBool(3);
                    bool isChildLevel = importer.GetBool(4);

                    int level;
                    if (string.IsNullOrWhiteSpace(levelText) || !int.TryParse(levelText, out level) || level < 1)
                    {
                        continue;
                    }

                    context.RegionLevels.AddOrUpdate(p => p.Level,
                        new RegionLevel()
                        {
                            Level = level,
                            Label = regionLabel,
                            Abbreviation = regionAbbreviation,
                            IsRequired = isRequired,
                            IsChildLevel = isChildLevel
                        });
                    count++;
                }

                context.SaveChanges();
            }
        }

        private void LoadRegions(string filename)
        {
            DomainContext context = new DomainContext();

            Dictionary<int, Region> regionDict = new Dictionary<int, Region>();
            
            string dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "Region"))
            {
                int count = 1;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    string uniqueId = importer.GetString(0);
                    int uniqueNum;
                    if (string.IsNullOrWhiteSpace(uniqueId) || !int.TryParse(uniqueId, out uniqueNum) || uniqueNum < 1)
                    {
                        continue;
                    }

                    int level = importer.GetInt(1);
                    string code = importer.GetString(2);
                    string name = importer.GetString(3);
                    int parentNum = importer.GetInt(4);

                    Region reg = context.Regions.Include(p => p.ParentRegion).FirstOrDefault(p => p.Level == level && p.Code == code);
                    if (reg == null)
                    {
                        reg = context.Regions.Create();
                        reg.Level = level;
                        reg.Code = code;
                        context.Regions.Add(reg);
                    }
                    reg.Name = name;
                    Region parentRegion = null;

                    if (parentNum > 0 && regionDict.TryGetValue(parentNum, out parentRegion))
                    {
                        reg.ParentRegion = parentRegion;
                    }
                    else
                    {
                        reg.ParentRegion = null;
                    }

                    regionDict.Add(uniqueNum, reg);
                    count++;                    
                }

                context.SaveChanges();
            }

        }
        
        private void LoadRegionAreas(string filename)
        {
            DomainContext context = new DomainContext();
            CommonContext commonContext = new CommonContext();

            // Need to delete all region areas first...
            context.Database.ExecuteSqlCommand($"DELETE FROM {context.GetTableName<RegionArea>()}");
            
            string dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "RegionAreas"))
            {
                int count = 1;

                while (count <= _args.MaxCount && importer.GetNextRow())
                {
                    string levelText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(levelText))
                    {
                        continue;
                    }

                    int level;
                    if (!int.TryParse(levelText, out level) || level == 0)
                    {
                        continue;
                    }

                    string parentCode = importer.GetString(1);
                    string regionCode = importer.GetString(2);
                    string countryCode = importer.GetString(4);
                    string stateCode = importer.GetString(5);
                    string countyCode = importer.GetString(6);
                    string city = importer.GetString(7);
                    string ziplow = importer.GetString(8);
                    string ziphigh = importer.GetString(9);
                    int priority = importer.GetInt(10);
                    Region region = null, parent = null;
                    Country country = null;
                    State state = null;
                    County county = null;

                    if (level == 1)
                    {
                        region = context.Regions.FirstOrDefault(p => p.Level == 1 && p.Code == regionCode);
                    }
                    else if (level == 2)
                    {
                        parent = context.Regions.FirstOrDefault(p => p.Level == 1 && p.Code == parentCode);
                        if (parent == null)
                        {
                            importer.LogError($"Parent code \"{parentCode}\" not defined.");
                            continue;
                        }
                        region = context.Regions.FirstOrDefault(p => p.Level == 2 && p.ParentRegionId == parent.Id && p.Code == regionCode);
                    }

                    if (region == null)
                    {
                        importer.LogError($"Region code \"{regionCode}\" parent \"{parentCode}\" not defined.");
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(countryCode))
                    {
                        country = commonContext.Countries.Local.FirstOrDefault(p => p.CountryCode == countryCode) ??
                                  commonContext.Countries.FirstOrDefault(p => p.CountryCode == countryCode);
                        if (country == null)
                        {
                            importer.LogError($"Invalid country code \"{countryCode}\".");
                            continue;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(stateCode) && country != null)
                    {
                        var states = commonContext.Entry(country).Collection(p => p.States);
                        if (!states.IsLoaded)
                        {
                            states.Load();
                        }

                        state = country.States.FirstOrDefault(p => p.StateCode == stateCode);
                        if (state == null)
                        {
                            importer.LogError($"Invalid state code \"{stateCode}\".");
                            continue;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(countyCode) && state != null)
                    {
                        var counties = commonContext.Entry(state).Collection(p => p.Counties);
                        if (!counties.IsLoaded)
                        {
                            counties.Load();
                        }

                        county = state.Counties.FirstOrDefault(p => p.FIPSCode == countyCode);
                        if (county == null)
                        {
                            importer.LogError($"Invalid county code \"{countyCode}\".");
                            continue;
                        }

                    }

                    RegionArea reg = new RegionArea();
                    reg.Region = region;
                    reg.Level = level;
                    reg.Country = country;
                    reg.State = state;
                    reg.County = county;
                    reg.City = city;
                    reg.PostalCodeHigh = ziphigh;
                    reg.PostalCodeLow = ziplow;
                    reg.Priority = priority;
                    context.RegionAreas.Add(reg);

                    count++;
                }

                context.SaveChanges();
            }
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

        private void LoadPrefixes(string filename)
        {
            DomainContext context = new DomainContext();
            string dataFile = Path.Combine(_crmDirectory, filename);

            // Force loading of genders
            context.Genders.ToList();        

            using (var importer = new FileImport(dataFile, "Prefix"))
            {
                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    string name = importer.GetString(1);
                    string label = importer.GetString(2);
                    string labelAbbreviation = importer.GetString(3);
                    string salutation = importer.GetString(4);
                    string gender = importer.GetString(5);
                    bool showOnline = importer.GetBool(6);
                    Gender g1 = null;

                    if (!string.IsNullOrWhiteSpace(gender))
                    {
                        g1 = context.Genders.Local.FirstOrDefault(p => p.Code == gender);
                    }
                                        
                    Prefix prefix = new Prefix();

                    prefix.Code = code;
                    prefix.Name = name;
                    prefix.LabelPrefix = label;
                    prefix.LabelAbbreviation = labelAbbreviation;
                    prefix.Salutation = salutation;
                    prefix.Gender = g1;
                    prefix.ShowOnline = showOnline;

                    context.Prefixes.AddOrUpdate(
                        p => p.Code,
                        prefix);
                }

            }

            context.SaveChanges();

        }

        private void LoadConstituents(string filename)
        {
            DomainContext context = new DomainContext();

            string dataFile = "";
            dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "Individual"))
            {
                int count = 0;

                while (count <= _args.MaxCount && importer.GetNextRow())
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
                    string employmentStartDate = importer.GetString(23);
                    string employmentEndDate = importer.GetString(24);
                    string firstEmploymentDate = importer.GetString(25);
                    bool isClientEmployee = (importer.GetString(26) == "yes");
                    string profession = importer.GetString(27);
                    string clergyType = importer.GetString(28);
                    string clergyStatus = importer.GetString(29);
                    string ordinationDate = importer.GetString(30);
                    string ordinationPlace = importer.GetString(31);
                    string prospectDate = importer.GetString(32);
                    int maritalStatus = importer.GetInt(33);
                    string marriageDate = importer.GetString(34);
                    string divorceDate = importer.GetString(35);
                    string deceasedDate = importer.GetString(36);
                    int birthMonth = importer.GetInt(37);
                    int birthDay = importer.GetInt(38);
                    int birthYear1 = importer.GetInt(39);
                    int birthYear2 = importer.GetInt(40);
                    string deletionCode = importer.GetString(41);
                    string deleteDate = importer.GetString(42);

                    string formattedName = firstName;
                    if (middleName != null)
                    {
                        formattedName = formattedName + " " + middleName;
                    }
                    if (lastName != null)
                    {
                        formattedName = formattedName + " " + lastName;
                    }

                    Console.WriteLine("{0}: {1}", count, constituentId);

                    Constituent constituent = new Constituent();

                    if (birthDay != 0 && birthMonth != 0 && birthYear1 != 0)
                    {
                        constituent.BirthDateType = BirthDateType.FullDate;
                    }
                    else if (birthDay != 0 && birthMonth != 0)
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
                    constituent.BirthDay = birthDay;
                    constituent.BirthMonth = birthMonth;
                    constituent.BirthYearFrom = birthYear1;
                    constituent.BirthYearTo = birthYear2;
                    constituent.Business = profession;
                    if (!string.IsNullOrWhiteSpace(clergyStatus))
                    {
                        try
                        {
                            constituent.ClergyStatus = context.ClergyStatuses.First(p => p.Code == clergyStatus);
                        }
                        catch (Exception e)
                        { }
                    }
                    if (!string.IsNullOrWhiteSpace(clergyType))
                    {
                        try
                        {
                            constituent.ClergyType = context.ClergyTypes.First(p => p.Code == clergyType);
                        }
                        catch (Exception e)
                        { }
                    }
                    constituent.ConstituentNumber = constituentId;
                    if (((constituentType == "F" || constituentType == "I") && deceasedDate != "?")
                        || deleteDate != "?" 
                        || !string.IsNullOrWhiteSpace(deletionCode) )
                    {
                        constituent.ConstituentStatus = context.ConstituentStatuses.First(p => p.Code == "IN");
                    }
                    else
                    {
                        constituent.ConstituentStatus = context.ConstituentStatuses.First(p => p.Code == "AC");
                    }
                    if (!string.IsNullOrWhiteSpace(constituentType))
                    {
                        try
                        {
                            constituent.ConstituentType = context.ConstituentTypes.First(p => p.Code == constituentType);
                        }
                        catch (Exception e)
                        { }
                    }
                    switch (correspondencePreference)
                    {
                        case "P":
                            constituent.CorrespondencePreference = CorrespondencePreference.Paper;
                            break;
                        case "E":
                            constituent.CorrespondencePreference = CorrespondencePreference.Email;
                            break;
                        case "EP":
                            constituent.CorrespondencePreference = CorrespondencePreference.Both;
                            break;
                        default:
                            constituent.CorrespondencePreference = CorrespondencePreference.None;
                            break;
                    }
                    if (deceasedDate != "?")
                    {
                        try
                        {
                            DateTime decDate;
                            DateTime.TryParse(deceasedDate, out decDate);
                            constituent.DeceasedDate = decDate;
                        }
                        catch (Exception e)
                        { }
                    }

//TODO:                    constituent.DisplayName = 

                    if (divorceDate != "?")
                    {
                        try
                        {
                            DateTime divDate;
                            DateTime.TryParse(divorceDate, out divDate);
                            constituent.DivorceDate = divDate;
                        }
                        catch (Exception e)
                        { }
                    }

                    if (!string.IsNullOrWhiteSpace(educationLevel))
                    {
                        try
                        {
                            constituent.EducationLevel = context.EducationLevels.First(p => p.Code == educationLevel);
                        }
                        catch (Exception e)
                        { }
                    }
                    constituent.Employer = employer;

                    if (employmentStartDate != "?")
                    {
                        try
                        {
                            DateTime empStDt;
                            DateTime.TryParse(employmentStartDate, out empStDt);
                            constituent.EmploymentStartDate = empStDt;
                        }
                        catch (Exception e)
                        { }
                    }

                    if (employmentEndDate != "?")
                    {
                        try
                        {
                            DateTime empEndDt;
                            DateTime.TryParse(employmentEndDate, out empEndDt);
                            constituent.EmploymentEndDate = empEndDt;
                        }
                        catch (Exception e)
                        { }
                    }

                    if (firstEmploymentDate != "?")
                    {
                        try
                        {
                            DateTime firstEmpDt;
                            DateTime.TryParse(firstEmploymentDate, out firstEmpDt);
                            constituent.FirstEmploymentDate = firstEmpDt;
                        }
                        catch (Exception e)
                        { }
                    }
                    constituent.FirstName = firstName;
                    //TODO:                    constituent.FormattedName = formattedName;
                    //TODO: constituent.IncomeLevel = 
                    constituent.IsEmployee = isClientEmployee;
                    constituent.LastName = lastName;
                    // TODO: constituent.MaritalStatus = 

                    if (marriageDate != "?")
                    {
                        try
                        {
                            DateTime marrDt;
                            DateTime.TryParse(marriageDate, out marrDt);
                            constituent.MarriageDate = marrDt;
                        }
                        catch (Exception e)
                        { }
                    }
                    constituent.MiddleName = middleName;
                    constituent.Name2= name2;
                    constituent.NameFormat = nameFormat;
                    constituent.Nickname = nickname;
                    
                    if (ordinationDate != "?")
                    {
                        try
                        {
                            DateTime ordDt;
                            DateTime.TryParse(ordinationDate, out ordDt);
                            constituent.OrdinationDate = ordDt;
                        }
                        catch (Exception e)
                        { }
                    }
                    constituent.PlaceOfOrdination = ordinationPlace;
                    constituent.Position = position;
                    if (!string.IsNullOrWhiteSpace(prefix))
                    {
                        try
                        {
                            constituent.Prefix = context.Prefixes.First(p => p.Code == prefix);
                        }
                        catch (Exception e)
                        { }
                    }

                    if (!string.IsNullOrWhiteSpace(profession))
                    {
                        try
                        {
                            constituent.Profession = context.Professions.First(p => p.Code == profession);
                        }
                        catch (Exception e)
                        { }
                    }

                    if (prospectDate != "?")
                    {
                        try
                        {
                            DateTime prosDt;
                            DateTime.TryParse(prospectDate, out prosDt);
                            constituent.ProspectDate = prosDt;
                        }
                        catch (Exception e)
                        { }
                    }

                    if (!string.IsNullOrWhiteSpace(salutationText))
                    {
                        constituent.Salutation = salutationText;
                    }
                    else
                    {
                        //TODO: get formatted salutation to save in constituent.Salutation
                    }

                    switch (salutationFormat)
                    {
                        case "0":
                            constituent.SalutationType = SalutationType.Default;
                            break;
                        case "1":
                            constituent.SalutationType = SalutationType.Formal;
                            break;
                        case "2":
                            constituent.SalutationType = SalutationType.Informal;
                            break;
                        case "3":
                            constituent.SalutationType = SalutationType.FormalSeparate;
                            break;
                        case "4":
                            constituent.SalutationType = SalutationType.InformalSeparate;
                            break;
                        default:
                            constituent.SalutationType = SalutationType.Custom;
                            break;
                    }

                    constituent.Source = sourceCode;
                    constituent.Suffix = suffix;
                    constituent.TaxId = taxId;

                    context.Constituents.AddOrUpdate(
                        p => p.ConstituentNumber,
                        constituent);

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
