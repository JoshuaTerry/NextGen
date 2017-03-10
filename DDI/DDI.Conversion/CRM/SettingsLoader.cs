using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Core;
using DDI.Business.CRM;
using DDI.Conversion.Statics;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.Common;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Models.Common;
using DDI.Shared.Extensions;
using DDI.Shared.Models;

namespace DDI.Conversion.CRM
{    
    internal class SettingsLoader : ConversionBase
    {
        public enum ConversionMethod
        {
            Codes = 200001,
            ContactTypes,
            Prefixes,
            RegionLevels,
            Regions,
            RegionAreas,
            RelationshipTypes,
            Tags,
            Configuration,
        }

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
        private const int CUSTOM_FIELD_SET = 1900;
        private const int CUSTOM_FIELD_VALUE_SET1 = 1901;
        private const int CUSTOM_FIELD_VALUE_SET2 = 1902;
        private const int CUSTOM_FIELD_VALUE_SET3 = 1903;
        private const int CUSTOM_FIELD_VALUE_SET4 = 1904;
        private const int CUSTOM_FIELD_VALUE_SET5 = 1905;
        private const int CUSTOM_FIELD_VALUE_SET6 = 1906;
        private const int CUSTOM_FIELD_VALUE_SET7 = 1907;
        private const int CUSTOM_FIELD_VALUE_SET8 = 1908;

        private string _crmDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, DirectoryName.CRM);

            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes(InputFile.CRM_NACodes));
            RunConversion(ConversionMethod.ContactTypes, () => LoadContactTypes(InputFile.CRM_ContactType));
            RunConversion(ConversionMethod.Prefixes, () => LoadPrefixes(InputFile.CRM_NamePrefix));
            RunConversion(ConversionMethod.RegionLevels, () => LoadRegionLevels(InputFile.CRM_RegionLevel));
            RunConversion(ConversionMethod.Regions, () => LoadRegions(InputFile.CRM_Region));
            RunConversion(ConversionMethod.RegionAreas, () => LoadRegionAreas(InputFile.CRM_RegionAreas));
            RunConversion(ConversionMethod.RelationshipTypes, () => LoadRelationshipTypes(InputFile.CRM_RelationshipType));
            RunConversion(ConversionMethod.Tags, () => LoadTags(InputFile.CRM_TagGroup, InputFile.CRM_TagCode));
            RunConversion(ConversionMethod.Configuration, () => LoadConfiguration(InputFile.CRM_NASetup));
        }


        private void LoadLegacyCodes(string filename)
        {
            DomainContext context = new DomainContext();
            int createdByField = 9;
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
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

                    bool masculine = true;
                    IAuditableEntity entity;

                    switch (codeSet)
                    {
                        case ADDRESS_TYPE_SET:
                            entity = new AddressType { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.AddressTypes.AddOrUpdate(prop => prop.Code, (AddressType)entity);
                            break;
                        case CLERGY_STATUS_SET:
                            entity = new ClergyStatus { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.ClergyStatuses.AddOrUpdate(p => p.Code, (ClergyStatus)entity);
                               
                            break;
                        case CLERGY_TYPE_SET:
                            entity = new ClergyType { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.ClergyTypes.AddOrUpdate(p => p.Code, (ClergyType)entity);                               
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
                            entity = new Denomination { Code = code, Name = description, IsActive = active, Religion = religion, Affiliation = affiliation };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Denominations.AddOrUpdate(p => p.Code, (Denomination)entity);                                
                            break;
                        case EDUCATION_LEVEL_SET:
                            entity = new EducationLevel { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.EducationLevels.AddOrUpdate(p => p.Code, (EducationLevel)entity);                                
                            break;
                        case ETHNICITY_SET:
                            entity = new Ethnicity { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Ethnicities.AddOrUpdate(p => p.Code, (Ethnicity)entity);                               
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
                            entity = new Gender { Code = code, Name = description, IsMasculine = masculine, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Genders.AddOrUpdate(p => p.Code, (Gender)entity);                               
                            break;
                        case LANGUAGE_SET:
                            entity = new Language { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Languages.AddOrUpdate(p => p.Code, (Language)entity);
                            break;
                        case DELETION_SET:
                            if (code == "AC" || code == "BL" || code == "HO" || code == "DEL")
                            {
                                continue;
                            }
                            entity = new ConstituentStatus() { Code = code, Name = description, BaseStatus = ConstituentBaseStatus.Inactive, IsActive = active, IsRequired = false };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.ConstituentStatuses.AddOrUpdate(p => p.Code, (ConstituentStatus)entity);
                            break;
                        case PROFESSION_SET:
                            entity = new Profession { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Professions.AddOrUpdate(p => p.Code, (Profession)entity);
                            break;
                        case EARNINGS_SET:
                            entity = new IncomeLevel { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.IncomeLevels.AddOrUpdate(p => p.Code, (IncomeLevel)entity);
                            break;
                        case MARITAL_STATUS_SET:
                            entity = new MaritalStatus { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.MaritalStatuses.AddOrUpdate(p => p.Code, (MaritalStatus)entity);
                            break;
                        case SCHOOL_SET:
                            entity = new School { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Schools.AddOrUpdate(p => p.Code, (School)entity);
                            break;
                        case DEGREE_SET:
                            entity = new Degree { Code = code, Name = description, IsActive = active };
                            ImportCreatedModifiedInfo(entity, importer, createdByField);
                            context.Degrees.AddOrUpdate(p => p.Code, (Degree)entity);
                            break;                        
                        case CUSTOM_FIELD_SET:
                            LoadCustomField(context, code, description, int1, int2, active);
                            break;
                        case CUSTOM_FIELD_VALUE_SET1:
                            LoadCustomFieldOption(context, 1, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET2:
                            LoadCustomFieldOption(context, 2, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET3:
                            LoadCustomFieldOption(context, 3, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET4:
                            LoadCustomFieldOption(context, 4, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET5:
                            LoadCustomFieldOption(context, 5, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET6:
                            LoadCustomFieldOption(context, 6, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET7:
                            LoadCustomFieldOption(context, 7, code, description);
                            break;
                        case CUSTOM_FIELD_VALUE_SET8:
                            LoadCustomFieldOption(context, 8, code, description);
                            break;
                        
                    }
                }
            }
            context.SaveChanges();
        }

        private void LoadCustomField(DomainContext context, string code, string description, int minValue, int maxValue, bool isActive)
        {
            // If there's no description, ignore the code.
            if (string.IsNullOrWhiteSpace(description))
            {
                return;
            }

            int displayOrder = 0;
            string displayOrderText;
            int displayOffset = 0;
            CustomFieldType customFieldType = CustomFieldType.TextBox;

            // Convert code (like CHAR01) to data type and display order.
            if (code.StartsWith("CHAR"))
            {
                customFieldType = CustomFieldType.TextBox;
                displayOrderText = code.Substring(4, 2);
            }
            else if (code.StartsWith("DATE"))
            {
                customFieldType = CustomFieldType.Date;
                displayOrderText = code.Substring(4, 2);
                displayOffset = 8;
            }
            else if (code.StartsWith("DEC"))
            {
                customFieldType = CustomFieldType.Number;
                displayOrderText = code.Substring(3, 2);
                displayOffset = 10;
            }
            else
            {
                return;
            }

            if (!int.TryParse(displayOrderText, out displayOrder))
            {
                return;
            }

            displayOrder += displayOffset;

            var customField = context.CustomField.FirstOrDefault(p => p.FieldType == customFieldType && p.DisplayOrder == displayOrder && p.Entity == CustomFieldEntity.CRM);
            if (customField == null)
            {
                customField = new CustomField();
                context.CustomField.Add(customField);
                customField.FieldType = customFieldType;
                customField.DisplayOrder = displayOrder;
                customField.Entity = CustomFieldEntity.CRM;
            }

            customField.IsActive = isActive;
            customField.LabelText = description;
            if (customFieldType == CustomFieldType.Number)
            {
                customField.MinValue = minValue.ToString();
                customField.MaxValue = maxValue.ToString();
                customField.DecimalPlaces = 2;
            }
            else
            {
                customField.MinValue = string.Empty;
                customField.MaxValue = string.Empty;
                customField.DecimalPlaces = 0;
            }
        }

        private void LoadCustomFieldOption(DomainContext context, int displayOrder, string code, string description)
        {
            var customFields = context.CustomField.Local;

            var customField = customFields.FirstOrDefault(p => p.FieldType == CustomFieldType.TextBox && p.DisplayOrder == displayOrder && p.Entity == CustomFieldEntity.CRM)
                                ??
                              customFields.FirstOrDefault(p => p.FieldType == CustomFieldType.DropDown && p.DisplayOrder == displayOrder && p.Entity == CustomFieldEntity.CRM)
                                ??
                              context.CustomField.FirstOrDefault(p => p.FieldType == CustomFieldType.TextBox && p.DisplayOrder == displayOrder && p.Entity == CustomFieldEntity.CRM)
                                ??
                              context.CustomField.FirstOrDefault(p => p.FieldType == CustomFieldType.DropDown && p.DisplayOrder == displayOrder && p.Entity == CustomFieldEntity.CRM);
            if (customField == null)
            {
                return;
            }

            var option = context.CustomFieldOption.Include(p => p.CustomField).FirstOrDefault(p => p.CustomField.Id == customField.Id && p.Code == code);
            if (option == null)
            {
                option = new CustomFieldOption();
                context.CustomFieldOption.Add(option);
                option.CustomField = customField;
                option.Code = code;
            }
            option.Description = description;

            // Force custom field to "DropDown".
            customField.FieldType = CustomFieldType.DropDown;
        }

        private void LoadConfiguration(string filename)
        {
            var bl = new ConfigurationLogic();
            IUnitOfWork uow = bl.UnitOfWork;
            
            using (var ifile = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                while (ifile.GetNextRow())
                {
                    CRMConfiguration config = bl.GetConfiguration<CRMConfiguration>();

                    // Salutations and formatting

                    ApplySalutationSettings(uow, "O", ifile.GetString(0), ifile.GetString(1));
                    ApplySalutationSettings(uow, "C", ifile.GetString(2), ifile.GetString(3));
                    ApplySalutationSettings(uow, "F", ifile.GetString(4), ifile.GetString(5));

                    string defaultSalutationFormat = ifile.GetString(6);
                    if (defaultSalutationFormat == "1")
                    {
                        defaultSalutationFormat = "Dear {FULL}";
                    }
                    else
                    {
                        defaultSalutationFormat = "Dear {P}{L}";
                    }

                    SalutationType defaultSalType = SalutationType.Formal;

                    switch (ifile.GetInt(7))
                    {
                        case 1: defaultSalType = SalutationType.Formal; break;
                        case 2: defaultSalType = SalutationType.Informal; break;
                        case 3: defaultSalType = SalutationType.FormalSeparate; break;
                        case 4: defaultSalType = SalutationType.InformalSeparate; break;
                    }

                    foreach (ConstituentType ct in uow.Where<ConstituentType>(p => p.Code == "I"))
                    {
                        ct.SalutationFormal = defaultSalutationFormat;
                    }

                    config.OmitInactiveSpouse = ifile.GetBool(8);
                    config.AddFirstNamesToSpouses = ifile.GetBool(9);
                    config.DefaultSalutationType = defaultSalType;

                    // Address types
                    config.HomeAddressTypes = GetAddressTypes(uow, ifile.GetString(10));
                    config.MailAddressTypes = GetAddressTypes(uow, ifile.GetString(11));

                    string defaultAddressType = ifile.GetCode(12);
                    config.DefaultAddressType = uow.FirstOrDefault<AddressType>(p => p.Code == defaultAddressType);

                    // Deceased status code and tag
                    string deceasedCode = ifile.GetCode(18);
                    string deceasedTag = ifile.GetCode(19);

                    if (string.IsNullOrWhiteSpace(deceasedCode))
                    {
                        config.DeceasedStatus = null;
                    }
                    else
                    {
                        config.DeceasedStatus = uow.FirstOrDefault<ConstituentStatus>(p => p.Code == deceasedCode);
                    }

                    config.DeceasedTags = new List<Tag>();

                    if (!string.IsNullOrWhiteSpace(deceasedTag))
                    {
                        var tag = uow.FirstOrDefault<Tag>(p => p.Code == deceasedTag);
                        if (tag != null)
                        {
                            config.DeceasedTags.Add(tag);
                        }
                    }

                    // Name format
                    string nameFormat = ifile.GetString(20);
                    if (!string.IsNullOrWhiteSpace(nameFormat))
                    {
                        var ct = uow.FirstOrDefault<ConstituentType>(p => p.Code == "I");
                        if (ct != null)
                        {
                            ct.NameFormat = ConvertNameFormat(nameFormat);
                        }

                    }

                    // Misc. flags
                    config.UseRegionSecurity = ifile.GetBool(21);
                    config.ApplyDeceasedTag = ifile.GetBool(22);

                    // Spouse relationships
                    string spouseCodes = ifile.GetString(23);
                    if (string.IsNullOrWhiteSpace(spouseCodes))
                    {
                        spouseCodes = "SPOU";
                    }

                    foreach (string entry in spouseCodes.Split(','))
                    {
                        string relationshipCode = entry.ToUpper().Trim();
                        RelationshipType relationshipType = uow.FirstOrDefault<RelationshipType>(p => p.Code == relationshipCode);
                        if (relationshipType != null)
                        {
                            relationshipType.IsSpouse = true;
                        }
                    }
                    
                    bl.SaveConfiguration(config);
                    break;
                }
            }
        }

        private void ApplySalutationSettings(IUnitOfWork uow, string code, string formal, string informal)
        {
            ConstituentType ct = uow.FirstOrDefault<ConstituentType>(p => p.Code == code);
            if (ct != null)
            {
                if (!string.IsNullOrWhiteSpace(formal))
                {
                    ct.SalutationFormal = formal.TrimEnd(',', ':');
                }
                if (!string.IsNullOrWhiteSpace(informal))
                {
                    ct.SalutationInformal = informal.TrimEnd(',', ':');
                }
            }
        }

        private IList<AddressType> GetAddressTypes(IUnitOfWork uow, string text)
        {
            List<AddressType> list = new List<AddressType>();

            foreach (var entry in text.Split(','))
            {
                if (!string.IsNullOrWhiteSpace(entry))
                {
                    string trimmedEntry = entry.Trim().ToUpper();
                    AddressType type = uow.FirstOrDefault<AddressType>(p => p.Code == trimmedEntry);
                    if (type != null)
                    {
                        list.Add(type);
                    }
                }
            }
            return list;
        }

        private string ConvertNameFormat(string text)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
            {
                if (char.IsLetter(c))
                    sb.Append('{').Append(Char.ToUpper(c)).Append('}');
                else if (c == '.')
                    sb.Insert(sb.Length - 1, 'I');
            }
            return sb.ToString();
        }

        private void LoadContactTypes(string filename)
        {
            DomainContext context = new DomainContext();
            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
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

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
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

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
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
                        reg.IsActive = true;
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

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
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

        private void LoadRelationshipTypes(string filename)
        {
            DomainContext context = new DomainContext();            

            List<Tuple<string,string,bool>> fixups = new List<Tuple<string, string, bool>>(); // 0:code, 1:reciprocal-code, 2:isMale
            RelationshipType rtype;

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    if (string.IsNullOrWhiteSpace(code) || code == "Code")
                    {
                        continue;
                    }

                    string name = importer.GetString(1);
                    string reciprocalMale = importer.GetString(2);
                    string reciprocalFemale = importer.GetString(3);
                    bool isSpouse = importer.GetBool(4);
                    bool isForIndividuals = importer.GetBool(5);
                    string category = importer.GetString(6);


                    rtype = new RelationshipType();
                    rtype.Code = code;
                    rtype.Name = name;
                    rtype.IsSpouse = isSpouse;
                    rtype.ConstituentCategory = (isForIndividuals ? ConstituentCategory.Individual : ConstituentCategory.Both);
                    rtype.IsActive = true;

                    if (!string.IsNullOrWhiteSpace(reciprocalMale))
                    {
                        fixups.Add(new Tuple<string, string, bool>(code, reciprocalMale, true));
                    }

                    if (!string.IsNullOrWhiteSpace(reciprocalFemale))
                    {
                        fixups.Add(new Tuple<string, string, bool>(code, reciprocalFemale, false));
                    }

                    if (!string.IsNullOrWhiteSpace(category))
                    {
                        var relCat = context.RelationshipCategories.FirstOrDefault(p => p.Code == category);
                        if (relCat == null)
                        {
                            importer.LogError($"Invalid relationship category {category} for {code}.");
                        }
                        else
                        {
                            rtype.RelationshipCategoryId = relCat.Id;
                        } 
                    }

                    context.RelationshipTypes.AddOrUpdate(p => p.Code, rtype);
                    count++;
                }

                context.SaveChanges();

                // Populate reciprocal relationships.

                foreach (var entry in fixups)
                {
                    rtype = context.RelationshipTypes.Local.FirstOrDefault(p => p.Code == entry.Item1);
                    if (rtype != null)
                    {
                        RelationshipType other = context.RelationshipTypes.Local.FirstOrDefault(p => p.Code == entry.Item2);
                        if (other == null)
                        {
                            importer.LogError($"Invalid relationship type reciprocal ${entry.Item2} for ${entry.Item1}.");
                        }
                        else if (entry.Item3)
                        {
                            rtype.ReciprocalTypeMaleId = other.Id;
                        }
                        else
                        {
                            rtype.ReciprocalTypeFemaleId = other.Id;
                        }                            
                    }
                }

                context.SaveChanges();
            }
        }


        private void LoadTags(string groupFilename, string tagFilename)
        {
            DomainContext context = new DomainContext();
            // Need to delete all tag related tables.
            context.Database.ExecuteSqlCommand($"DELETE FROM TagConstituents"); // Currently no way to retrieve these many-to-many table names.
            context.Database.ExecuteSqlCommand($"DELETE FROM TagConstituentTypes"); // Currently no way to retrieve these many-to-many table names.
            context.Database.ExecuteSqlCommand($"DELETE FROM {context.GetTableName<Tag>()}");
            context.Database.ExecuteSqlCommand($"DELETE FROM {context.GetTableName<TagGroup>()}");

            Dictionary<int, TagGroup> groupDict = new Dictionary<int, TagGroup>();

            // Load tag groups.
            using (var importer = CreateFileImporter(_crmDirectory, groupFilename, typeof(ConversionMethod)))
            {
                int count = 1;

                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);

                    string groupNumText = importer.GetString(0);
                    int groupNum = 0;
                    if (string.IsNullOrWhiteSpace(groupNumText) || !int.TryParse(groupNumText, out groupNum) || groupNum == 0)
                    {
                        continue;
                    }

                    TagGroup grp = new TagGroup();
                    grp.Name = importer.GetString(1);
                    grp.Order = importer.GetInt(2);
                    grp.IsActive = true;
                    grp.TagSelectionType = (importer.GetBool(3) ? TagSelectionType.Single : TagSelectionType.Multiple);
                    context.TagGroups.Add(grp);
                    groupDict.Add(groupNum, grp);

                    count++;
                }
                context.SaveChanges();

            }


            // Load Tags.
            using (var importer = CreateFileImporter(_crmDirectory, tagFilename, typeof(ConversionMethod)))
            {
                while (importer.GetNextRow())
                {
                    string code = importer.GetString(0);
                    string name = importer.GetString(1);
                    int order = importer.GetInt(2);
                    int groupNum = importer.GetInt(3);
                    bool isActive = importer.GetBool(8);

                    if (string.IsNullOrWhiteSpace(code) || order == 0)
                    {
                        continue;
                    }

                    Tag tag = new Tag();
                    tag.Code = code;
                    tag.Name = name;
                    tag.IsActive = isActive;
                    tag.Order = order;
                    tag.TagGroup = groupDict.GetValueOrDefault(groupNum);

                    context.Tags.Add(tag);
                }
                context.SaveChanges();
            }
        }

        private void LoadPrefixes(string filename)
        {
            DomainContext context = new DomainContext();

            // Force loading of genders
            var genders = context.Genders.ToList();

            using (var importer = CreateFileImporter(_crmDirectory, filename, typeof(ConversionMethod)))
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
                        g1 = genders.FirstOrDefault(p => p.Code == gender);
                    }
                                        
                    Prefix prefix = new Prefix();

                    prefix.Code = code;
                    prefix.Name = name;
                    prefix.LabelPrefix = label;
                    prefix.LabelAbbreviation = labelAbbreviation;
                    prefix.Salutation = salutation;
                    prefix.Gender = g1;
                    prefix.GenderId = g1?.Id;
                    prefix.ShowOnline = showOnline;
                    prefix.IsActive = true;

                    context.Prefixes.AddOrUpdate(
                        p => p.Code,
                        prefix);
                }

            }

            context.SaveChanges();
        }

    }
}
