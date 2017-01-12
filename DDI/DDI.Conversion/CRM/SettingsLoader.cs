﻿using System;
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
using DDI.Shared.ModuleInfo;

namespace DDI.Conversion.CRM
{
    [ModuleType(Shared.Enums.ModuleType.CRM)]
    internal class SettingsLoader :  ConversionBase
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

        private string _crmDirectory;

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            _crmDirectory = Path.Combine(baseDirectory, "CRM");

            RunConversion(ConversionMethod.Codes, () => LoadLegacyCodes("NACodes.csv"));
            RunConversion(ConversionMethod.ContactTypes, () => LoadContactTypes("ContactType.csv"));
            RunConversion(ConversionMethod.Prefixes, () => LoadPrefixes("NamePrefix.csv"));
            RunConversion(ConversionMethod.RegionLevels, () => LoadRegionLevels("RegionLevel.csv"));
            RunConversion(ConversionMethod.Regions, () => LoadRegions("Region.csv"));
            RunConversion(ConversionMethod.RegionAreas, () => LoadRegionAreas("RegionAreas.csv"));
            RunConversion(ConversionMethod.RelationshipTypes, () => LoadRelationshipTypes("RelationshipType.csv"));
            RunConversion(ConversionMethod.Tags, () => LoadTags("TagGroup.csv", "TagCode.csv"));
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

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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

            string dataFile = Path.Combine(_crmDirectory, filename);
            using (var importer = new FileImport(dataFile, "RelType"))
            {
                int count = 1;

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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
            string dataFile = Path.Combine(_crmDirectory, groupFilename);
            using (var importer = new FileImport(dataFile, "TagGroup"))
            {
                int count = 1;

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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
            dataFile = Path.Combine(_crmDirectory, tagFilename);
            using (var importer = new FileImport(dataFile, "Tag"))
            {
                int count = 1;

                while (count <= MethodArgs.MaxCount && importer.GetNextRow())
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

    }
}
