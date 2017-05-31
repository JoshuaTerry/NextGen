using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class ContactCategoryDataSource
    {
        public static IList<ContactCategory> GetDataSource(IUnitOfWork uow)
        {
            IList<ContactCategory> existing = uow.GetRepositoryDataSource<ContactCategory>();
            if (existing != null)
            {
                return existing;
            }

            var list = new List<ContactCategory>();
            list.Add(new ContactCategory() { Code = "N", Name = "Person", SectionTitle = "Point of Contact", TextBoxLabel = "Name", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ContactCategory() { Code = "S", Name = "Social", SectionTitle = "Social Media", TextBoxLabel = "URL", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ContactCategory() { Code = "W", Name = "Web", SectionTitle = "Web sites", TextBoxLabel = "URL", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ContactCategory() { Code = "E", Name = "Email", SectionTitle = "Emails", TextBoxLabel = "Email", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ContactCategory() { Code = "O", Name = "Other", SectionTitle = "Other Contacts", TextBoxLabel = "Info", Id = GuidHelper.NewSequentialGuid() });
            list.Add(new ContactCategory() { Code = "P", Name = "Phone", SectionTitle = "Phone Numbers", TextBoxLabel = "Phone", Id = GuidHelper.NewSequentialGuid() });

            uow.CreateRepositoryForDataSource(list);
            return list;
        }    

    }

    
}
