using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class PrefixDataSource
    {

        public static IList<Prefix> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<Prefix> existing = uow.GetRepositoryOrNull<Prefix>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }


            var prefixes = new List<Prefix>();
            prefixes.Add(NewPrefix(uow, "Mr", "Mr.", "Mr.", "Mr.", "", "M"));
            prefixes.Add(NewPrefix(uow, "Miss", "Miss", "Miss", "Miss", "", "F"));
            prefixes.Add(NewPrefix(uow, "Ms", "Ms.", "Ms.", "Ms.", "", "F"));
            prefixes.Add(NewPrefix(uow, "Mrs", "Mrs.", "Mrs.", "Mrs.", "", "F"));
            prefixes.Add(NewPrefix(uow, "Rev", "Rev.", "The Reverend", "Rev.", "", ""));
            prefixes.Add(NewPrefix(uow, "Dr", "Dr.", "Dr.", "Dr.", "", ""));
            prefixes.Add(NewPrefix(uow, "Law", "Lawyer", "{NAME}, Esq.", "{NAME}, Esq.", "Dear {MR} {NAME}", ""));
            uow.CreateRepositoryForDataSource(prefixes);
            return prefixes;
        }    

        private static Prefix NewPrefix(IUnitOfWork uow, string code, string name, string labelPrefix, string labelAbbreviation, string salutation, string genderCode)
        {
            var prefix = new Prefix()
            {
                Code = code,
                LabelAbbreviation = labelAbbreviation,
                LabelPrefix = labelPrefix,
                Name = name,
                Salutation = salutation,
                Id = GuidHelper.NextGuid()
            };
            if (!string.IsNullOrWhiteSpace(genderCode))
            {
                prefix.Gender = uow.FirstOrDefault<Gender>(p => p.Code == genderCode);
            }
            return prefix;
        }

    }

    
}
