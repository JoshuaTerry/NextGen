using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.ModuleInfo;

namespace DDI.Conversion.CRM
{
    [ModuleType(Shared.Enums.ModuleType.CRM)]
    internal class ConstituentConverter : ConversionBase
    {

        public enum ConversionMethod
        {
            Individuals = 200100,            
            Organizations,
            Addresses,
            ConstituentAddresses,
            DoingBusinessAs,
            Education,
            AlternateIDs,
            ContactInformation,
            PaymentPreferences,
            Relationships
        }

        private const string CONSTITUENT_ID_FILE = "ConstituentId.csv";
        private const string ADDRESS_ID_FILE = "AddressId.csv";

        private string _crmDirectory;
        private string _outputDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, "CRM");
            _outputDirectory = Path.Combine(_crmDirectory, "Output");
            Directory.CreateDirectory(_outputDirectory);

            RunConversion(ConversionMethod.Individuals, () => LoadIndividuals("Individual.csv", false));
            RunConversion(ConversionMethod.Individuals, () => LoadIndividuals("IndividualFW.csv", true));
            RunConversion(ConversionMethod.Organizations, () => LoadOrganizations("Organization.csv", true));
            RunConversion(ConversionMethod.Organizations, () => LoadOrganizations("OrganizationFW.csv", true));
            RunConversion(ConversionMethod.Addresses, () => LoadAddresses("Address.csv", false));
            RunConversion(ConversionMethod.Addresses, () => LoadAddresses("AddressFW.csv", true));
            RunConversion(ConversionMethod.ConstituentAddresses, () => LoadConstituentAddress("ConstituentAddress.csv", false));
            RunConversion(ConversionMethod.ConstituentAddresses, () => LoadConstituentAddress("ConstituentAddressFW.csv", true));

            //RunConversion(ConversionMethod.DoingBusinessAs, () => LoadDoingBusinessAs("");
        }



        private void LoadAddresses(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            CommonContext commonContext = new CommonContext();

            context.RegionLevels.Load();
            context.Regions.Load();

            var countries = commonContext.Countries.Local;
            var states = commonContext.States.Local;
            var counties = commonContext.Counties.Local;
            var regionLevels = context.RegionLevels.Local;
            var regions = context.Regions.Local;

            RegionLevel regionLevel1 = regionLevels.FirstOrDefault(p => p.Level == 1);
            RegionLevel regionLevel2 = regionLevels.FirstOrDefault(p => p.Level == 2);
            RegionLevel regionLevel3 = regionLevels.FirstOrDefault(p => p.Level == 3);
            RegionLevel regionLevel4 = regionLevels.FirstOrDefault(p => p.Level == 4);

            FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, ADDRESS_ID_FILE), append, true);
            FileExport<Address> addressFile = new FileExport<Address>(Path.Combine(_outputDirectory, "Address.csv"), append);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 0;

                if (!append)
                {
                    addressFile.AddHeaderRow();
                }

                while (importer.GetNextRow())
                {
                    int legacyId = importer.GetInt(0);
                    string streetAddress1 = importer.GetString(1);
                    string streetAddress2 = importer.GetString(2);
                    string countryCode = importer.GetString(3);
                    string stateCode = importer.GetString(4);
                    string countyFips = importer.GetString(5);
                    string postalCode = importer.GetString(6);
                    string city = importer.GetString(7);
                    string region1 = importer.GetString(8);
                    string region2 = importer.GetString(9);
                    string region3 = importer.GetString(10);
                    string region4 = importer.GetString(11);

                    count++;

                    Country country = null;
                    if (string.IsNullOrWhiteSpace(countryCode) || countryCode == "US")
                    {
                        countryCode = "USA";
                    }

                    country = country = countries.FirstOrDefault(p => p.LegacyCode == countryCode) ??
                        commonContext.Countries.Include(p => p.States).FirstOrDefault(p => p.LegacyCode == countryCode);                

                    State state = null;
                    if (!string.IsNullOrWhiteSpace(stateCode) && country != null)
                    {
                        state = country.States.FirstOrDefault(p => p.StateCode == stateCode);
                        if (state != null && state.Counties == null)
                        {
                            commonContext.Entry(state).Collection(p => p.Counties).Load();
                        }
                    }

                    County county = null;
                    if (countryCode == "USA" && !string.IsNullOrWhiteSpace(countyFips) && state != null)
                    {                        
                        county = state.Counties.FirstOrDefault(p => p.FIPSCode == countyFips);
                    }

                    Region regionOne = null;
                    if (!string.IsNullOrWhiteSpace(region1))
                    {
                        regionOne = regions.FirstOrDefault(p => p.Level == 1 && p.Code == region1);
                    }

                    Region regionTwo = null;
                    if (!string.IsNullOrWhiteSpace(region2) && regionLevel2 != null)
                    {
                        if (regionLevel1.IsChildLevel)
                        {
                            regionTwo = regionOne?.ChildRegions.FirstOrDefault(p => p.Code == region2);
                        }
                        else
                        {
                            regionTwo = regions.FirstOrDefault(p => p.Level == 2 && p.Code == region2);
                        }
                    }

                    Region regionThree = null;
                    if (!string.IsNullOrWhiteSpace(region3) && regionLevel3 != null)
                    {
                        if (regionLevel3.IsChildLevel)
                        {
                            regionThree = regionTwo.ChildRegions.FirstOrDefault(p => p.Code == region3);
                        }
                        else
                        {
                            regionThree = regions.FirstOrDefault(p => p.Level == 3 &&  p.Code == region3);
                        }                        
                    }

                    Region regionFour = null;
                    if (!string.IsNullOrWhiteSpace(region4) && regionLevel4 != null)
                    {
                        if (regionLevel4.IsChildLevel)
                        {
                            regionFour = regionThree.ChildRegions.FirstOrDefault(p => p.Code == region4);
                        }
                        else
                        {
                            regionFour = regions.FirstOrDefault(p => p.Level == 4 && p.Code == region4);
                        }
                    }

                    Address address = new Address();
                    address.AssignPrimaryKey();
                    address.LegacyKey = legacyId;
                    address.AddressLine1 = streetAddress1;
                    address.AddressLine2 = streetAddress2;
                    address.City = city;
                    address.CountryId = country?.Id;
                    address.StateId = state?.Id;
                    address.CountyId = county?.Id;
                    address.PostalCode = postalCode;
                    address.Region1Id = regionOne?.Id;
                    address.Region2Id = regionTwo?.Id;
                    address.Region3Id = regionThree?.Id;
                    address.Region4Id = regionFour?.Id;

                    addressFile.AddRow(address);

                    legacyIdFile.AddRow(new LegacyToID(legacyId, address.Id));

                    if (count % 1000 == 0)
                    {
                        importer.LogMessage($"{count} Loaded");
                        addressFile.Flush();
                        legacyIdFile.Flush();
                    }

                }
            }

            addressFile.Dispose();
            legacyIdFile.Dispose();
        }

        private ObservableCollection<T> LoadEntities<T>(DbSet<T> entities, params string[] paths) where T : class
        {
            IQueryable<T> query = entities;
            foreach (string path in paths)
            {
                query = query.Include(path);
            }
            query.Load();            
            return entities.Local;
        }

        private void LoadIndividuals(string filename, bool append)
        {
            char[] commaDelimiter = { ',' };
            NameFormatter nameFormatter;

            DomainContext context = new DomainContext();

            UnitOfWorkEF uow = new UnitOfWorkEF(context);
            nameFormatter = uow.GetBusinessLogic<NameFormatter>();

            // Load entity sets that will be queried often...
            var constituentTypes = LoadEntities(context.ConstituentTypes);
            var ethnicities = LoadEntities(context.Ethnicities);
            var denominations = LoadEntities(context.Denominations);
            var genders = LoadEntities(context.Genders);
            var prefixes = LoadEntities(context.Prefixes, "Gender");
            var incomelevels = LoadEntities(context.IncomeLevels);
            var educationLevels = LoadEntities(context.EducationLevels);
            var professions = LoadEntities(context.Professions);
            var clergyTypes = LoadEntities(context.ClergyTypes);
            var clergyStatuses = LoadEntities(context.ClergyStatuses);
            var maritalStatuses = LoadEntities(context.MaritalStatuses);
            var constituentStatuses = LoadEntities(context.ConstituentStatuses);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, CONSTITUENT_ID_FILE), append, true);
                FileExport<Constituent> constituentFile = new FileExport<Constituent>(Path.Combine(_outputDirectory, "Constituent.csv"), append);
                FileExport<JoinRow> ethnicityFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, "EthnicityConstituents.csv"), append);
                FileExport<JoinRow> denominationFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, "DenominationConstituents.csv"), append);

                ethnicityFile.SetColumnNames("Ethnicity_Id", "Constituent_Id");
                denominationFile.SetColumnNames("Denomination_Id", "Constituent_Id");

                int count = 0;
                if (!append)
                {
                    constituentFile.AddHeaderRow();
                    ethnicityFile.AddHeaderRow();
                    denominationFile.AddHeaderRow();
                }

                while (importer.GetNextRow())
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
                    string ethnicityCode = importer.GetString(6);
                    string denominationCode = importer.GetString(7);
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

                    ConstituentType constituentType = constituentTypes.FirstOrDefault(p => p.Code == constituentTypeCode);
                    if (constituentType == null)
                    {
                        importer.LogError($"PIN {constituentNum} has invalid constituent type \"{constituentType}\".");
                        continue;
                    }
                    
                    Constituent constituent = null;
                    constituent = new Constituent();
                    constituent.AssignPrimaryKey();

                    constituent.ConstituentNumber = constituentNum;
                    constituent.ConstituentType = constituentType;
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
                    string[] codelist = ethnicityCode.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string code in codelist)
                    {
                        Ethnicity ethnicity = ethnicities.FirstOrDefault(p => p.Code == code);
                        if (ethnicity == null)
                        {
                            importer.LogError($"Invalid ethnicity code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            ethnicityFile.AddRow(new JoinRow(ethnicity.Id, constituent.Id));
                        }
                    }

                    // Denominations
                    codelist = denominationCode.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string code in codelist)
                    {
                        Denomination denomination = context.Denominations.Local.FirstOrDefault(p => p.Code == code);
                        if (denomination == null)
                        {
                            importer.LogError($"Invalid denomination code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            denominationFile.AddRow(new JoinRow(denomination.Id, constituent.Id));
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
                    if (!string.IsNullOrWhiteSpace(prefixCode))
                    {
                        Prefix prefix = prefixes.FirstOrDefault(p => p.Code == prefixCode);
                        if (prefix == null)
                        {
                            importer.LogError($"Invalid prefix code {prefixCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.PrefixId = prefix.Id;
                            constituent.Prefix = prefix;
                        }
                    }

                    // Gender
                    if (!string.IsNullOrWhiteSpace(genderCode))
                    {
                        Gender gender = genders.FirstOrDefault(p => p.Code == genderCode);
                        if (gender == null)
                        {
                            importer.LogError($"Invalid gender code {genderCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.GenderId = gender.Id;
                            constituent.Gender = gender;
                        }
                    }

                    // Try to assign gender based on prefix.
                    if (constituent.Gender == null && constituent.Prefix != null)
                    {
                        constituent.Gender = constituent.Prefix.Gender;
                        constituent.GenderId = constituent.Gender?.Id;
                    }

                    // Earnings
                    if (!string.IsNullOrWhiteSpace(earningsCode))
                    {
                        IncomeLevel incomeLevel = incomelevels.FirstOrDefault(p => p.Code == earningsCode);
                        if (incomeLevel == null)
                        {
                            importer.LogError($"Invalid earnings code {earningsCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.IncomeLevelId = incomeLevel.Id;
                        }
                    }

                    // Education Level
                    if (!string.IsNullOrWhiteSpace(educationLevelCode))
                    {
                        EducationLevel educationLevel = educationLevels.FirstOrDefault(p => p.Code == educationLevelCode);
                        if (educationLevel == null)
                        {
                            importer.LogError($"Invalid education level code {educationLevelCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.EducationLevelId = educationLevel.Id;
                        }
                    }

                    // Profession
                    if (!string.IsNullOrWhiteSpace(professionCode))
                    {
                        Profession profession = professions.FirstOrDefault(p => p.Code == professionCode);
                        if (profession == null)
                        {
                            importer.LogError($"Invalid profession code {professionCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.ProfessionId = profession.Id;
                        }
                    }

                    // Clergy Type
                    if (!string.IsNullOrWhiteSpace(clergyTypeCode))
                    {
                        ClergyType clergyType = clergyTypes.FirstOrDefault(p => p.Code == clergyTypeCode);
                        if (clergyType == null)
                        {
                            importer.LogError($"Invalid clergy type code {clergyTypeCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.ClergyTypeId = clergyType.Id;
                        }
                    }

                    // Clergy Status
                    if (!string.IsNullOrWhiteSpace(clergyStatusCode))
                    { 
                        ClergyStatus clergyStatus = clergyStatuses.FirstOrDefault(p => p.Code == clergyStatusCode);
                        if (clergyStatus == null)
                        {
                            importer.LogError($"Invalid clergy status code {clergyStatusCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.ClergyStatusId = clergyStatus.Id;
                        }
                    }

                    // Constituent Status

                    if (!string.IsNullOrWhiteSpace(deletionCode))
                    {
                        ConstituentStatus constituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == deletionCode);
                        if (constituentStatus == null)
                        {
                            importer.LogError($"Invalid constituent status code {deletionCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.ConstituentStatus = constituentStatus;
                        }
                    }

                    // Anything coming over with a deletion date should be set to status deleted.
                    if (deleteDate.HasValue)
                    {
                        constituent.ConstituentStatusDate = deleteDate;

                        if (constituent.ConstituentStatus == null)
                        {
                            constituent.ConstituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_DELETED);                            
                        }
                    }
                    else
                    {
                        if (constituent.ConstituentStatus == null)
                        {
                            // If no status, set to active.
                            constituent.ConstituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_ACTIVE);
                        }
                        else
                        {
                            constituent.ConstituentStatusDate = DateTime.Parse("1/1/1990");
                        }
                    }

                    constituent.ConstituentStatusId = constituent.ConstituentStatus?.Id;

                    // Marital Status
                    if (!string.IsNullOrWhiteSpace(maritalStatusCode))
                    {
                        MaritalStatus maritalStatus = maritalStatuses.FirstOrDefault(p => p.Code == maritalStatusCode);
                        if (maritalStatus == null)
                        {
                            importer.LogError($"Invalid marital status code {maritalStatusCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.MaritalStatusId = maritalStatus.Id;
                        }
                    }


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

                    LabelFormattingOptions options = new LabelFormattingOptions() { OmitPrefix = true, Recipient = LabelRecipient.Primary };
                    string nameLine1, nameLine2;

                    nameFormatter.BuildNameLines(constituent, null, options, out nameLine1, out nameLine2);
                    constituent.FormattedName = nameLine1;

                    constituentFile.AddRow(constituent);
                    legacyIdFile.AddRow(new LegacyToID(constituent.ConstituentNumber, constituent.Id));

                    if (count % 1000 == 0)
                    {
                        importer.LogMessage($"{count} Loaded {constituentNum}: {nameLine1}");

                        constituentFile.Flush();
                        denominationFile.Flush();
                        ethnicityFile.Flush();
                        legacyIdFile.Flush();
                    }

                }
                constituentFile.Dispose();
                denominationFile.Dispose();
                ethnicityFile.Dispose();
                legacyIdFile.Dispose();
            }
           
        }

        private void LoadOrganizations(string filename, bool append)
        {
            char[] commaDelimiter = { ',' };
            DomainContext context = new DomainContext();

            // Load entity sets that will be queried often...
            var constituentTypes = LoadEntities(context.ConstituentTypes);
            var constituentStatuses = LoadEntities(context.ConstituentStatuses);
            var ethnicities = LoadEntities(context.Ethnicities);
            var denominations = LoadEntities(context.Denominations);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, CONSTITUENT_ID_FILE), append, true);
                FileExport<Constituent> constituentFile = new FileExport<Constituent>(Path.Combine(_outputDirectory, "Constituent.csv"), append);
                FileExport<JoinRow> ethnicityFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, "EthnicityConstituents.csv"), append);
                FileExport<JoinRow> denominationFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, "DenominationConstituents.csv"), append);

                ethnicityFile.SetColumnNames("Ethnicity_Id", "Constituent_Id");
                denominationFile.SetColumnNames("Denomination_Id", "Constituent_Id");

                int count = 0;
                if (!append)
                {
                    constituentFile.AddHeaderRow();
                    ethnicityFile.AddHeaderRow();
                    denominationFile.AddHeaderRow();
                }

                while (importer.GetNextRow())
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
                    string ethnicityCode = importer.GetString(6);
                    string denominationCode = importer.GetString(7);
                    string correspondencePreference = importer.GetString(8);
                    string salutationFormat = importer.GetString(9);
                    string salutationText = importer.GetString(10);
                    string business = importer.GetString(11);
                    bool isTaxExempt = importer.GetBool(12);
                    bool isLetterReceived = importer.GetBool(13);
                    DateTime? taxExemptDate = importer.GetDateTime(14);
                    int membership = importer.GetInt(15);
                    int yearEstablished = importer.GetInt(16);
                    string deletionCode = importer.GetString(17);
                    DateTime? deleteDate = importer.GetDateTime(18);
                    if (deletionCode == "YBD") deletionCode = "DEL";

                    ConstituentType constituentType = constituentTypes.FirstOrDefault(p => p.Code == constituentTypeCode);
                    if (constituentType == null)
                    {
                        importer.LogError($"PIN {constituentNum} has invalid constituent type \"{constituentType}\".");
                        continue;
                    }

                    Constituent constituent = null;
                    constituent = new Constituent();
                    constituent.AssignPrimaryKey();

                    constituent.ConstituentNumber = constituentNum;
                    constituent.ConstituentType = constituentType;
                    constituent.ConstituentTypeId = constituentType.Id;
                    constituent.Name = name;
                    constituent.Name2 = name2;
                    constituent.Source = sourceCode;
                    constituent.TaxId = taxId;
                    constituent.Salutation = salutationText;
                    constituent.Business = business;
                    constituent.IsTaxExempt = isTaxExempt;
                    constituent.IsIRSLetterReceived = isLetterReceived;
                    constituent.TaxExemptVerifyDate = taxExemptDate;
                    constituent.MembershipCount = membership;
                    constituent.YearEstablished = yearEstablished;

                    // Ethnicity
                    string[] codelist = ethnicityCode.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string code in codelist)
                    {
                        Ethnicity ethnicity = ethnicities.FirstOrDefault(p => p.Code == code);
                        if (ethnicity == null)
                        {
                            importer.LogError($"Invalid ethnicity code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            ethnicityFile.AddRow(new JoinRow(ethnicity.Id, constituent.Id));
                        }
                    }

                    // Denominations
                    codelist = denominationCode.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string code in codelist)
                    {
                        Denomination denomination = context.Denominations.Local.FirstOrDefault(p => p.Code == code);
                        if (denomination == null)
                        {
                            importer.LogError($"Invalid denomination code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            denominationFile.AddRow(new JoinRow(denomination.Id, constituent.Id));
                        }
                    }

                    // Correspondence preference
                    switch (correspondencePreference)
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


                    // Constituent Status

                    if (!string.IsNullOrWhiteSpace(deletionCode))
                    {
                        ConstituentStatus constituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == deletionCode);
                        if (constituentStatus == null)
                        {
                            importer.LogError($"Invalid constituent status code {deletionCode} for PIN {constituentNum}.");
                        }
                        else
                        {
                            constituent.ConstituentStatus = constituentStatus;
                        }
                    }

                    // Anything coming over with a deletion date should be set to status deleted.
                    if (deleteDate.HasValue)
                    {
                        constituent.ConstituentStatusDate = deleteDate;

                        if (constituent.ConstituentStatus == null)
                        {
                            constituent.ConstituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_DELETED);
                        }
                    }
                    else
                    {
                        if (constituent.ConstituentStatus == null)
                        {
                            // If no status, set to active.
                            constituent.ConstituentStatus = constituentStatuses.FirstOrDefault(p => p.Code == Initialize.CONSTITUENT_STATUS_ACTIVE);
                        }
                        else
                        {
                            constituent.ConstituentStatusDate = DateTime.Parse("1/1/1990");
                        }
                    }

                    constituent.ConstituentStatusId = constituent.ConstituentStatus?.Id;

                    constituent.FormattedName = constituent.Name;

                    constituentFile.AddRow(constituent);
                    legacyIdFile.AddRow(new LegacyToID(constituent.ConstituentNumber, constituent.Id));

                    if (count % 1000 == 0)
                    {
                        importer.LogMessage($"{count} Loaded {constituentNum}: {name}");

                        constituentFile.Flush();
                        denominationFile.Flush();
                        ethnicityFile.Flush();
                        legacyIdFile.Flush();
                    }
                }

                constituentFile.Dispose();
                denominationFile.Dispose();
                ethnicityFile.Dispose();
                legacyIdFile.Dispose();
            }

        }


        private void LoadConstituentAddress(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            var addressTypes = LoadEntities<AddressType>(context.AddressTypes);

            // Load the constituent Ids
            Dictionary<string, Guid> constituentIds = LoadLegacyIds(_outputDirectory, CONSTITUENT_ID_FILE);
            Dictionary<string, Guid> addresIds = LoadLegacyIds(_outputDirectory, ADDRESS_ID_FILE);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<ConstituentAddress>(Path.Combine(_outputDirectory, "ConstituentAddress.csv"), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int legacyAddressId;
                    string legacyAddressText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(legacyAddressText) || !int.TryParse(legacyAddressText, out legacyAddressId) || legacyAddressId <= 0)
                    {
                        continue;
                    }

                    string constituentNum = importer.GetString(1);
                    string addressTypeCode = importer.GetString(2);

                    Guid addressId = addresIds.GetValueOrDefault(legacyAddressText);
                    if (addressId == default(Guid))
                    {
                        importer.LogError($"Invalid address legacy ID {legacyAddressText}.");
                        continue;
                    }

                    Guid constituentId = constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    AddressType addressType = addressTypes.FirstOrDefault(p => p.Code == addressTypeCode);
                    if (addressType == null)
                    {
                        importer.LogError($"Invalid address type \"{addressTypeCode}\".");
                        continue;
                    }

                    ConstituentAddress constituentAddress = new ConstituentAddress();
                    constituentAddress.AssignPrimaryKey();
                    constituentAddress.ConstituentId = constituentId;
                    constituentAddress.AddressId = addressId;                   
                    constituentAddress.AddressTypeId = addressType.Id;
                    constituentAddress.Comment = importer.GetString(3);
                    constituentAddress.StartDate = importer.GetDateTime(4);
                    constituentAddress.EndDate = importer.GetDateTime(5);
                    constituentAddress.StartDay = importer.GetInt(6);
                    constituentAddress.EndDay = importer.GetInt(7);
                    constituentAddress.IsPrimary = importer.GetBool(8);

                    string residentType = importer.GetString(9);
                    switch (residentType)
                    {
                        case "Y": constituentAddress.ResidentType = ResidentType.Primary; break;
                        case "N": constituentAddress.ResidentType = ResidentType.Secondary; break;
                        case "S": constituentAddress.ResidentType = ResidentType.Separate; break;
                        default:
                            importer.LogError($"Invalid resident type {residentType} for address ID {legacyAddressId} constituent ID {constituentNum}.");
                            break;
                    }

                    outputFile.AddRow(constituentAddress);

                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogMessage($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }


    }
}
