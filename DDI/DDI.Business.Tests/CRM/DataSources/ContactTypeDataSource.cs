using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class ContactTypeDataSource
    {
        public static IList<ContactType> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<ContactType> existing = uow.GetRepositoryOrNull<ContactType>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var list = new List<ContactType>();

            // Email
            ContactCategory category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Email);
            AddType(list, category, "H", "Home email", false, false, true);
            AddType(list, category, "C", "Contact email", false, false, false);
            AddType(list, category, "W", "Work email", false, false, false);

            // Phone
            category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Phone);
            AddType(list, category, "H", "Home phone", false, false, true);
            AddType(list, category, "F", "Fax #", false, false, false);
            AddType(list, category, "W", "Work phone", false, false, false);
            AddType(list, category, "M", "Mobile phone", false, false, false);
            AddType(list, category, "C", "Church phone", false, true, false);

            // Web
            category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Web);
            AddType(list, category, "H", "Home page", false, false, true);

            // Social
            category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Social);
            AddType(list, category, "F", "Facebook", false, false, false);

            // Person
            category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Person);
            AddType(list, category, "C", "Contact person", false, false, true);

            // Other
            category = uow.GetRepositoryOrNull<ContactCategory>()?.Entities.FirstOrDefault(p => p.Code == ContactCategoryCodes.Other);
            AddType(list, category, "M", "Mailing address", false, false, true);
            
            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

        private static void AddType(IList<ContactType> list, ContactCategory category, string code, string name, bool isAlwaysShown, bool canDelete, bool isDefault)
        {
            ContactType type = new ContactType()
            {
                Code = code,
                Name = name,
                ContactCategory = category,
                ContactCategoryId = category?.Id,
                IsAlwaysShown = isAlwaysShown,
                IsActive = true,
                CanDelete = canDelete,
                Id = GuidHelper.NewSequentialGuid()
            };

            if (isDefault && category != null)
            {
                category.DefaultContactType = type;
                category.DefaultContactTypeId = type.Id;
                type.ContactCategoryDefaults = new List<ContactCategory>() { category };
            }

            list.Add(type);
        }

    }

    
}
