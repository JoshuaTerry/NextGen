using System.Collections.Generic;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class PrefixDataSource
    {

        public static IList<Prefix> GetDataSource(IUnitOfWork uow)
        {
            IList<Prefix> existing = uow.GetRepositoryDataSource<Prefix>();
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
            prefixes.Add(NewPrefix(uow, "Drs", "Drs.", "Drs.", "Drs.", "", ""));
            prefixes.Add(NewPrefix(uow, "Law", "Lawyer", "{NAME}, Esq.", "{NAME}, Esq.", "Dear {MR} {NAME}", ""));
            prefixes.Add(NewPrefix(uow, "Abbot", "Abbot", "The Right Reverend", "Rt. Rev.", "Dear Father Abbot", "M"));
            prefixes.Add(NewPrefix(uow, "Mon", "Monsieur", "Mon.", "Mon.", "", "M"));
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
                Id = GuidHelper.NewSequentialGuid()
            };
            if (!string.IsNullOrWhiteSpace(genderCode))
            {
                prefix.Gender = uow.FirstOrDefault<Gender>(p => p.Code == genderCode);
            }
            return prefix;
        }

    }

    
}
