﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DDI.Conversion;
using DDI.Data;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;
//using DDI.Shared.ModuleInfo;

namespace DDI.Conversion.CRM
{
    /// <summary>
    /// Seeding of initial data for the CRM module.
    /// </summary>       
    internal class Initialize : ConversionBase
    {
        public enum ConversionMethod
        {
            Initialize = 200000,
        }

        public const string CONSTITUENT_STATUS_ACTIVE = "AC";
        public const string CONSTITUENT_STATUS_INACTIVE = "IN";
        public const string CONSTITUENT_STATUS_BLOCKED = "BL";
        public const string CONSTITUENT_STATUS_DELETED = "DEL";

        public override void Execute(string baseDirectory, IEnumerable<ConversionMethodArgs> conversionMethods)
        {
            MethodsToRun = conversionMethods;
            RunConversion(ConversionMethod.Initialize, () => LoadInitialData());
        }

        private void LoadInitialData()
        {
            DomainContext context = new DomainContext();

            //ConstituentStatuses
            context.CRM_ConstituentStatuses.AddOrUpdate(
                p => p.Code,
                new ConstituentStatus { Code = CONSTITUENT_STATUS_ACTIVE, Name = "Active", IsActive = true, BaseStatus = ConstituentBaseStatus.Active, IsRequired = true },
                new ConstituentStatus { Code = CONSTITUENT_STATUS_INACTIVE, Name = "Inactive", IsActive = true, BaseStatus = ConstituentBaseStatus.Inactive, IsRequired = true },
                new ConstituentStatus { Code = CONSTITUENT_STATUS_BLOCKED, Name = "Blocked", IsActive = true, BaseStatus = ConstituentBaseStatus.Blocked, IsRequired = true },
                new ConstituentStatus { Code = CONSTITUENT_STATUS_DELETED, Name = "Deleted", IsActive = true, BaseStatus = ConstituentBaseStatus.Inactive, IsRequired = true }
                );

            //Constituent Types
            context.CRM_ConstituentTypes.AddOrUpdate(
                p => p.Code,
                new ConstituentType { Category = ConstituentCategory.Individual, Code = "I", Name = "Individual", IsActive = true, IsRequired = true, NameFormat = "{PREFIX}{FIRST}{MI}{LAST}{SUFFIX}", SalutationFormal = "Dear {PREFIX}{LAST}", SalutationInformal = "Dear {NICKNAME}" },
                new ConstituentType { Category = ConstituentCategory.Organization, Code = "O", Name = "Organization", IsActive = true, IsRequired = true, SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" },
                new ConstituentType { Category = ConstituentCategory.Organization, Code = "C", Name = "Church", IsActive = true, IsRequired = true, SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" },
                new ConstituentType { Category = ConstituentCategory.Individual, Code = "F", Name = "Family", IsActive = true, IsRequired = true, NameFormat = "The {FIRST}{MI}{LAST} Family", SalutationFormal = "Dear Friends", SalutationInformal = "Dear Friends" }
            );

            //Genders
            context.CRM_Genders.AddOrUpdate(
                p => p.Code,
                new Gender { Code = "M", IsMasculine = true, Name = "Male", IsActive = true },
                new Gender { Code = "F", IsMasculine = false, Name = "Female", IsActive = true }
            );

            // Contact categories
            AddContactCategory(context, ContactCategory.EMAIL, "Email", "Emails", "Email");
            AddContactCategory(context, ContactCategory.PERSON, "Person", "Point of Contact", "Name");
            AddContactCategory(context, ContactCategory.PHONE, "Phone", "Phone Numbers", "Phone");
            AddContactCategory(context, ContactCategory.WEB, "Web", "Web sites", "URL");
            AddContactCategory(context, ContactCategory.SOCIAL, "Social", "Social Media", "URL");
            AddContactCategory(context, ContactCategory.OTHER, "Other", "Other Contacts", "Info");

            // Relationship categories
            AddRelationshipCategory(context, "G", "General Relationships", true);
            AddRelationshipCategory(context, "M", "Membership", false);

            context.SaveChanges();
        }

        private void AddContactCategory(DomainContext context, string code, string description, string title, string infoLabel)
        {
            context.CRM_ContactCategories.AddOrUpdate(p => p.Code,
                new ContactCategory()
                {
                    Code = code,
                    Name = description,
                    SectionTitle = title,
                    TextBoxLabel = infoLabel,
                    IsActive = true
                });
        }

        private void AddRelationshipCategory(DomainContext context, string code, string description, bool isShownInQuickView)
        {
            context.CRM_RelationshipCategories.AddOrUpdate(p => p.Code,
                new RelationshipCategory()
                {
                    Code = code,
                    Name = description,
                    IsShownInQuickView = isShownInQuickView,
                    IsActive = true
                });
        }



    }
}
