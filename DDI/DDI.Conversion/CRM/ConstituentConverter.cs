using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.CRM;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Extensions;
using DDI.Conversion.Core;

namespace DDI.Conversion.CRM
{    
 
    /// <summary>
    /// OpenEdge to SSIS Data Conversion for the CRM module.
    /// </summary>
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
            Relationships,
            Tags,
            CustomFieldData,
            Notes
        }

        private string _crmDirectory;
        private string _outputDirectory;

        private Dictionary<int, Guid> _constituentIds, _addressIds;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);
            _outputDirectory = Path.Combine(DirectoryName.OutputDirectory, DirectoryName.CRM);
            _constituentIds = new Dictionary<int, Guid>();
            _addressIds = new Dictionary<int, Guid>();

            // Make sure the IS Payload directory exists.
            Directory.CreateDirectory(_outputDirectory);
            
            RunConversion(ConversionMethod.Individuals, () => ConvertIndividuals(InputFile.CRM_Individual, false));
            RunConversion(ConversionMethod.Individuals, () => ConvertIndividuals(InputFile.CRM_IndividualFW, true));
            RunConversion(ConversionMethod.Organizations, () => ConvertOrganizations(InputFile.CRM_Organization, true));
            RunConversion(ConversionMethod.Organizations, () => ConvertOrganizations(InputFile.CRM_OrganizationFW, true));
            RunConversion(ConversionMethod.Addresses, () => ConvertAddresses(InputFile.CRM_Address, false));
            RunConversion(ConversionMethod.Addresses, () => ConvertAddresses(InputFile.CRM_AddressFW, true));
            RunConversion(ConversionMethod.ConstituentAddresses, () => ConvertConstituentAddress(InputFile.CRM_ConstituentAddress, false));
            RunConversion(ConversionMethod.ConstituentAddresses, () => ConvertConstituentAddress(InputFile.CRM_ConstituentAddressFW, true));

            RunConversion(ConversionMethod.DoingBusinessAs, () => ConvertDoingBusinessAs(InputFile.CRM_ConstituentDBA, false));
            RunConversion(ConversionMethod.Education, () => ConvertEducation(InputFile.CRM_EducationInfo, false));
            RunConversion(ConversionMethod.AlternateIDs, () => ConvertAlternateIds(InputFile.CRM_AlternateID, false));
            RunConversion(ConversionMethod.ContactInformation, () => ConvertContactInfo(InputFile.CRM_ContactInfo, false));
            RunConversion(ConversionMethod.ContactInformation, () => ConvertContactInfo(InputFile.CRM_ContactInfoFW, true));
            RunConversion(ConversionMethod.Relationships, () => ConvertRelationships(InputFile.CRM_Relationship, false));
            RunConversion(ConversionMethod.Tags, () => ConvertTags(InputFile.CRM_ConstituentTag, false));
            RunConversion(ConversionMethod.CustomFieldData, () => ConvertCustomData(InputFile.CRM_CustomData, false));
            RunConversion(ConversionMethod.Notes, () => ConvertNotes(InputFile.CRM_MemoConstituent, false));
        }

        private void ConvertNotes(string filename, bool append)
        {
            var noteConverter = new NoteConverter();
            noteConverter.ConvertNotes(() => CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)), "Constituent", null, false);
        }

        /// <summary>
        /// If necessary, load legacy constituent IDs into dictionary.
        /// </summary>
        private void LoadConstituentIds()
        {
            if (_constituentIds.Count == 0)
            {
                _constituentIds = LoadIntLegacyIds(_outputDirectory, OutputFile.CRM_ConstituentIdMappingFile);
            }
        }

        /// <summary>
        /// If necessary, load legacy address numbers IDs into dictionary.
        /// </summary>
        private void LoadAddressIds()
        {
            if (_addressIds.Count == 0)
            {
                _addressIds = LoadIntLegacyIds(_outputDirectory, OutputFile.CRM_AddressIdMappingFile);
            }
        }

        private void ConvertAddresses(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            CommonContext commonContext = new CommonContext();

            context.CRM_RegionLevels.Load();
            context.CRM_Regions.Load();

            var countries = commonContext.Countries.Local;
            var states = commonContext.States.Local;
            var counties = commonContext.Counties.Local;
            var regionLevels = context.CRM_RegionLevels.Local;
            var regions = context.CRM_Regions.Local;

            RegionLevel regionLevel1 = regionLevels.FirstOrDefault(p => p.Level == 1);
            RegionLevel regionLevel2 = regionLevels.FirstOrDefault(p => p.Level == 2);
            RegionLevel regionLevel3 = regionLevels.FirstOrDefault(p => p.Level == 3);
            RegionLevel regionLevel4 = regionLevels.FirstOrDefault(p => p.Level == 4);

            FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.CRM_AddressIdMappingFile), append, true);
            FileExport<Address> addressFile = new FileExport<Address>(Path.Combine(_outputDirectory, OutputFile.CRM_AddressFile), append);

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
                    string streetAddress1 = importer.GetString(1, 255);
                    string streetAddress2 = importer.GetString(2, 255);
                    string countryCode = importer.GetString(3);
                    string stateCode = importer.GetString(4);
                    string countyFips = importer.GetString(5);
                    string postalCode = importer.GetString(6);
                    string city = importer.GetString(7, 128);
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
                    _addressIds[legacyId] = address.Id;

                    if (count % 1000 == 0)
                    {
                        importer.LogDebug($"{count} Loaded");
                        addressFile.Flush();
                        legacyIdFile.Flush();
                    }

                }
            }

            addressFile.Dispose();
            legacyIdFile.Dispose();
        }

        private void ConvertIndividuals(string filename, bool append)
        {
            char[] commaDelimiter = { ',' };
            NameFormatter nameFormatter;

            DomainContext context = new DomainContext();

            UnitOfWorkEF uow = new UnitOfWorkEF(context);
            nameFormatter = uow.GetBusinessLogic<NameFormatter>();

            // Load entity sets that will be queried often...
            var constituentTypes = LoadEntities(context.CRM_ConstituentTypes);
            var ethnicities = LoadEntities(context.CRM_Ethnicities);
            var denominations = LoadEntities(context.CRM_Denominations);
            var genders = LoadEntities(context.CRM_Genders);
            var prefixes = LoadEntities(context.CRM_Prefixes, nameof(Prefix.Gender));
            var incomelevels = LoadEntities(context.CRM_IncomeLevels);
            var educationLevels = LoadEntities(context.CRM_EducationLevels);
            var professions = LoadEntities(context.CRM_Professions);
            var clergyTypes = LoadEntities(context.CRM_ClergyTypes);
            var clergyStatuses = LoadEntities(context.CRM_ClergyStatuses);
            var maritalStatuses = LoadEntities(context.CRM_MaritalStatuses);
            var constituentStatuses = LoadEntities(context.CRM_ConstituentStatuses);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.CRM_ConstituentIdMappingFile), append, true);
                FileExport<Constituent> constituentFile = new FileExport<Constituent>(Path.Combine(_outputDirectory, OutputFile.CRM_ConstituentFile), append);
                FileExport<JoinRow> ethnicityFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.CRM_EthnicityFile), append); // Join table created by EF
                FileExport<JoinRow> denominationFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.CRM_DenominationFile), append); // Join table created by EF

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
                    string name = importer.GetString(2, 255);
                    string name2 = importer.GetString(3, 128);
                    string sourceCode = importer.GetString(4);
                    string taxId = importer.GetString(5);
                    string ethnicityCode = importer.GetString(6);
                    string denominationCode = importer.GetString(7);
                    string correspondencePreference = importer.GetString(8);
                    string salutationFormat = importer.GetString(9);
                    string salutationText = importer.GetString(10, 255);
                    string prefixCode = importer.GetString(11);
                    string firstName = importer.GetString(12, 128);
                    string middleName = importer.GetString(13, 128);
                    string lastName = importer.GetString(14, 128);
                    string suffix = importer.GetString(15, 128);
                    string nickname = importer.GetString(16, 128);
                    string nameFormat = importer.GetString(17, 128);
                    string genderCode = importer.GetString(18);
                    string earningsCode = importer.GetString(19);
                    string educationLevelCode = importer.GetString(20);
                    string employer = importer.GetString(21, 128);
                    string position = importer.GetString(22, 128);
                    DateTime? employmentStartDate = importer.GetDate(23);
                    DateTime? employmentEndDate = importer.GetDate (24);
                    DateTime? firstEmploymentDate = importer.GetDate(25);
                    bool isClientEmployee = (importer.GetString(26) == "yes");
                    string professionCode = importer.GetString(27);
                    string clergyTypeCode = importer.GetString(28);
                    string clergyStatusCode = importer.GetString(29);
                    DateTime? ordinationDate = importer.GetDate(30);
                    string ordinationPlace = importer.GetString(31, 128);
                    DateTime? prospectDate = importer.GetDate(32);
                    string maritalStatusCode = importer.GetString(33);
                    DateTime? marriageDate = importer.GetDate(34);
                    DateTime? divorceDate = importer.GetDate(35);
                    DateTime? deceasedDate = importer.GetDate(36);
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
                        Denomination denomination = context.CRM_Denominations.Local.FirstOrDefault(p => p.Code == code);
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

                    nameFormatter.GetNameLines(constituent, null, options, out nameLine1, out nameLine2);
                    constituent.FormattedName = nameLine1;

                    constituentFile.AddRow(constituent);
                    legacyIdFile.AddRow(new LegacyToID(constituent.ConstituentNumber, constituent.Id));
                    _constituentIds[constituent.ConstituentNumber] = constituent.Id;

                    if (count % 1000 == 0)
                    {
                        importer.LogDebug($"{count} Loaded {constituentNum}: {nameLine1}");

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

        private void ConvertOrganizations(string filename, bool append)
        {
            char[] commaDelimiter = { ',' };
            DomainContext context = new DomainContext();

            // Load entity sets that will be queried often...
            var constituentTypes = LoadEntities(context.CRM_ConstituentTypes);
            var constituentStatuses = LoadEntities(context.CRM_ConstituentStatuses);
            var ethnicities = LoadEntities(context.CRM_Ethnicities);
            var denominations = LoadEntities(context.CRM_Denominations);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                FileExport<LegacyToID> legacyIdFile = new FileExport<LegacyToID>(Path.Combine(_outputDirectory, OutputFile.CRM_ConstituentIdMappingFile), append, true);
                FileExport<Constituent> constituentFile = new FileExport<Constituent>(Path.Combine(_outputDirectory, OutputFile.CRM_ConstituentFile), append);
                FileExport<JoinRow> ethnicityFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.CRM_EthnicityFile), append); // Join table created by EF
                FileExport<JoinRow> denominationFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.CRM_DenominationFile), append); // Join table created by EF

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
                    string name = importer.GetString(2, 255);
                    string name2 = importer.GetString(3, 128);
                    string sourceCode = importer.GetString(4);
                    string taxId = importer.GetString(5);
                    string ethnicityCode = importer.GetString(6);
                    string denominationCode = importer.GetString(7);
                    string correspondencePreference = importer.GetString(8);
                    string salutationFormat = importer.GetString(9);
                    string salutationText = importer.GetString(10, 255);
                    string business = importer.GetString(11, 128);
                    bool isTaxExempt = importer.GetBool(12);
                    bool isLetterReceived = importer.GetBool(13);
                    DateTime? taxExemptDate = importer.GetDateTime(14);
                    int membership = importer.GetInt(15);
                    int yearEstablished = importer.GetInt(16);
                    string deletionCode = importer.GetString(17);
                    DateTime? deleteDate = importer.GetDate(18);
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
                        Denomination denomination = context.CRM_Denominations.Local.FirstOrDefault(p => p.Code == code);
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
                    _constituentIds[constituent.ConstituentNumber] = constituent.Id;

                    if (count % 1000 == 0)
                    {
                        importer.LogDebug($"{count} Loaded {constituentNum}: {name}");

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

        private void ConvertConstituentAddress(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            var addressTypes = LoadEntities<AddressType>(context.CRM_AddressTypes);

            // Load the constituent Ids
            LoadConstituentIds();
            LoadAddressIds();

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<ConstituentAddress>(Path.Combine(_outputDirectory, OutputFile.CRM_ConstituentAddresFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int legacyAddressId;
                    string legacyAddressText = importer.GetString(0).Trim();
                    if (string.IsNullOrWhiteSpace(legacyAddressText) || !int.TryParse(legacyAddressText, out legacyAddressId) || legacyAddressId <= 0)
                    {
                        continue;
                    }

                    int constituentNum = importer.GetInt(1);
                    string addressTypeCode = importer.GetString(2);

                    Guid addressId = _addressIds.GetValueOrDefault(legacyAddressId);
                    if (addressId == default(Guid))
                    {
                        importer.LogError($"Invalid address legacy ID {legacyAddressText}.");
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
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
                    constituentAddress.StartDate = importer.GetDate(4);
                    constituentAddress.EndDate = importer.GetDate(5);
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
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }

        private void ConvertDoingBusinessAs(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            // Load the constituent Ids
            LoadConstituentIds();
            
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<DoingBusinessAs>(Path.Combine(_outputDirectory, OutputFile.CRM_DoingBusinessAsFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(constituentNumText) || !int.TryParse(constituentNumText, out constituentNum) || constituentNum <= 0)
                    {
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    DoingBusinessAs doingBusinessAs = new DoingBusinessAs();
                    doingBusinessAs.AssignPrimaryKey();
                    doingBusinessAs.ConstituentId = constituentId;
                    doingBusinessAs.Name = importer.GetString(1, 128);
                    doingBusinessAs.StartDate = importer.GetDate(2);
                    doingBusinessAs.EndDate = importer.GetDate(3);

                    outputFile.AddRow(doingBusinessAs);

                }

                outputFile.Dispose();
            }
        }

        private void ConvertEducation(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            var schools = LoadEntities(context.CRM_Schools);
            var degrees = LoadEntities(context.CRM_Degrees);

            // Load the constituent Ids
            LoadConstituentIds();

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<Education>(Path.Combine(_outputDirectory, OutputFile.CRM_EducationFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(constituentNumText) || !int.TryParse(constituentNumText, out constituentNum) || constituentNum <= 0)
                    {
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    string schoolCode = importer.GetString(5);
                    string degreeCode = importer.GetString(7);

                    School school = null;
                    Degree degree = null;

                    if (!string.IsNullOrWhiteSpace(schoolCode))
                    {
                        school = schools.FirstOrDefault(p => p.Code == schoolCode);
                        if (school == null)
                        {
                            importer.LogError($"Invalid school code \"{schoolCode}\".");
                            continue;
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(degreeCode))
                    {
                        degree = degrees.FirstOrDefault(p => p.Code == degreeCode);
                        if (degree == null)
                        {
                            importer.LogError($"Invalid degree code \"{degreeCode}\".");
                            continue;
                        }
                    }
                    
                    Education education = new Education();
                    education.AssignPrimaryKey();
                    education.ConstituentId = constituentId;
                    education.Major = string.Empty;
                    education.StartDate = importer.GetDate(2);
                    education.EndDate = importer.GetDate(4);
                    education.SchoolId = school?.Id;
                    education.SchoolOther = importer.GetString(6, 128);
                    education.DegreeId = degree?.Id;
                    education.DegreeOther = importer.GetString(8, 128);

                    outputFile.AddRow(education);
                }

                outputFile.Dispose();
            }
        }

        private void ConvertAlternateIds(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            // Load the constituent Ids
            LoadConstituentIds();

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<AlternateId>(Path.Combine(_outputDirectory, OutputFile.CRM_AlternateIdFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(constituentNumText) || !int.TryParse(constituentNumText, out constituentNum) || constituentNum <= 0)
                    {
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    string altText = importer.GetString(1, 20).Trim();
                    string altType = importer.GetString(2, 20).Trim();

                    // If the type code is non-blank and doesn't equal (None), prefix the alternate ID with the code.
                    if (!string.IsNullOrWhiteSpace(altType) && altType != "(None)")
                    {
                        altText = altType + altText;
                    }

                    AlternateId alternateId = new AlternateId();
                    alternateId.AssignPrimaryKey();
                    alternateId.ConstituentId = constituentId;
                    alternateId.Name = altText;

                    outputFile.AddRow(alternateId);

                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }

        private void ConvertContactInfo(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            // Load the constituent Ids
            LoadConstituentIds();
            
            // Load contact types
            var contactTypes = LoadEntities(context.CRM_ContactTypes, nameof(ContactType.ContactCategory));

            // Because contact info self-relates, conversion requires two passes.  The first pass builds a dictionary of Guids for the parents.
            var parentDict = new Dictionary<int, Guid>();
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    int legacyId = importer.GetInt(0);
                    if (legacyId == 0)
                    {
                        continue;
                    }

                    // Determine whether this is a parent contact info row.
                    bool isParent = importer.GetBool(9);
                    if (!isParent)
                    {
                        continue;
                    }

                    // Create a guid for this row.
                    Guid id = Guid.NewGuid();

                    // Create a dictionary entry.
                    parentDict[legacyId] = id;
                }
            }

            // Second pass performs the actual conversion.

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<ContactInfo>(Path.Combine(_outputDirectory, OutputFile.CRM_ContactInfoFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int legacyId = importer.GetInt(0);
                    if (legacyId == 0)
                    {
                        continue;
                    }

                    int constituentNum = 0;
                    string constituentNumText = importer.GetString(1);

                    // Skip only if constituent number column is non-blank and it can't be converted to int.  Zero pins will be flagged as an error below.
                    if (!string.IsNullOrWhiteSpace(constituentNumText) && !int.TryParse(constituentNumText, out constituentNum))
                    {                       
                        continue;
                    }

                    Guid constituentId;

                    constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    string categoryCode = importer.GetString(2);
                    string typeCode = importer.GetString(3);

                    ContactType contactType = contactTypes.FirstOrDefault(p => p.ContactCategory.Code == categoryCode && p.Code == typeCode);
                    if (contactType == null)
                    {
                        importer.LogError($"Invalid contact type for category \"{categoryCode}\", type \"{typeCode}.\"");
                        continue;
                    }

                    string infoText = importer.GetString(4, 128);
                    string comment = importer.GetString(5, 128);
                    bool isPreferred = importer.GetBool(6);
                    // column 7 is address number, which is no longer needed.
                    int parentLegacyId = importer.GetInt(8);
                    bool isParent = importer.GetBool(9);

                    ContactInfo contactInfo = new ContactInfo();
                    if (isParent)
                    {
                        // If this is a parent row, an ID has already been generated.
                        contactInfo.Id = parentDict[legacyId];
                    }
                    else
                    {
                        contactInfo.AssignPrimaryKey();
                    }

                    contactInfo.ConstituentId = constituentId;
                    contactInfo.Comment = comment;
                    contactInfo.ContactTypeId = contactType.Id;
                    contactInfo.Info = infoText;
                    contactInfo.IsPreferred = isPreferred;

                    if (parentLegacyId != 0)
                    {
                        contactInfo.ParentContactId = parentDict.GetValueOrDefault(parentLegacyId);
                        if (contactInfo.ParentContactId == default(Guid))
                        {
                            contactInfo.ParentContactId = null;
                        }
                    }
                    
                    outputFile.AddRow(contactInfo);

                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }

        private void ConvertRelationships(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            // Load the constituent Ids
            LoadConstituentIds();

            // Load the relationship types
            var types = LoadEntities<RelationshipType>(context.CRM_RelationshipTypes);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<Relationship>(Path.Combine(_outputDirectory, OutputFile.CRM_RelationshipFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum1, constituentNum2;
                    string constituentNum1Text = importer.GetString(0);
                    string constituentNum2Text = importer.GetString(1);
                    string typeCode = importer.GetString(2);

                    if (string.IsNullOrWhiteSpace(constituentNum1Text) || !int.TryParse(constituentNum1Text, out constituentNum1))
                    {
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(constituentNum2Text) || !int.TryParse(constituentNum2Text, out constituentNum2))
                    {
                        continue;
                    }

                    Guid constituentId1 = _constituentIds.GetValueOrDefault(constituentNum1);
                    if (constituentId1 == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum1}.");
                        continue;
                    }

                    Guid constituentId2 = _constituentIds.GetValueOrDefault(constituentNum2);
                    if (constituentId2 == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum2}.");
                        continue;
                    }

                    RelationshipType type = types.FirstOrDefault(p => p.Code == typeCode);
                    if (type == null)
                    {
                        importer.LogError($"Invalid relationship type code \"{typeCode}\".");
                        continue;
                    }

                    Relationship relationship = new Relationship();
                    relationship.AssignPrimaryKey();
                    relationship.Constituent1Id = constituentId1;
                    relationship.Constituent2Id = constituentId2;
                    relationship.RelationshipTypeId = type.Id;

                    outputFile.AddRow(relationship);

                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }


        private void ConvertTags(string filename, bool append)
        {
            DomainContext context = new DomainContext();
            char[] commaDelimiter = { ',' };

            // Load the constituent Ids
            LoadConstituentIds();

            // Load the tags
            var tags = LoadEntities(context.CRM_Tags);

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                // This is a join table created by EF.
                var outputFile = new FileExport<JoinRow>(Path.Combine(_outputDirectory, OutputFile.CRM_TagFile), append);
                outputFile.SetColumnNames("Tag_Id", "Constituent_Id");

                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                    string tagCodeList = importer.GetString(1);

                    if (string.IsNullOrWhiteSpace(constituentNumText) || !int.TryParse(constituentNumText, out constituentNum))
                    {
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    // The tags are comma delimited set of codes.  Output a row to the join table for each tag.
                    foreach (string code in tagCodeList.Split(commaDelimiter, StringSplitOptions.RemoveEmptyEntries).Distinct())
                    {
                        string codeUpper = code.ToUpper();
                        Tag tag = tags.FirstOrDefault(p => p.Code == codeUpper);
                        if (tag == null)
                        {
                            importer.LogError($"Invalid tag code \"{code}\" for PIN {constituentNum}.");
                        }
                        else
                        {
                            outputFile.AddRow(new JoinRow(tag.Id, constituentId));
                        }
                    }


                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }

        private void ConvertCustomData(string filename, bool append)
        {
            DomainContext context = new DomainContext();

            var customFields = LoadEntities(context.CustomField);

            // Load the constituent Ids
            LoadConstituentIds();

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                var outputFile = new FileExport<CustomFieldData>(Path.Combine(_outputDirectory, OutputFile.CRM_CustomDataFile), append);
                if (!append)
                {
                    outputFile.AddHeaderRow();
                }

                int count = 0;

                while (importer.GetNextRow())
                {
                    count++;

                    int constituentNum;
                    string constituentNumText = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(constituentNumText) || !int.TryParse(constituentNumText, out constituentNum))
                    {
                        continue;
                    }

                    Guid constituentId = _constituentIds.GetValueOrDefault(constituentNum);
                    if (constituentId == default(Guid))
                    {
                        importer.LogError($"Invalid constituent number {constituentNum}.");
                        continue;
                    }

                    // Iterate thru display positions 1 thru 12.
                    for (int position = 1; position <= 12; position++)
                    {
                        string valueText = importer.GetString(position, 255);
                        if (string.IsNullOrWhiteSpace(valueText))
                        {
                            continue;
                        }

                        // For numeric positions (11, 12) ignore zeros.
                        if (position >= 11 && position <= 12 && valueText == "0")
                        {
                            continue;
                        }

                        CustomField field = customFields.FirstOrDefault(p => p.DisplayOrder == position);
                        if (field == null)
                        {
                            importer.LogError($"No custom field defined for display order {position}.");
                            continue;
                        }
                        CustomFieldData data = new CustomFieldData();
                        data.AssignPrimaryKey();
                        data.CustomFieldId = field.Id;
                        data.ParentEntityId = constituentId;
                        data.EntityType = Shared.Enums.Common.CustomFieldEntity.CRM;
                        data.Value = valueText;

                        outputFile.AddRow(data);
                    }


                    if (count % 1000 == 0)
                    {
                        outputFile.Flush();
                        importer.LogDebug($"{count} Loaded");
                    }
                }

                outputFile.Dispose();
            }
        }


    }
}
