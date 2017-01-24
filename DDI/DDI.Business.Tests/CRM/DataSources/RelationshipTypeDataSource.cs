using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Enums.CRM;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class RelationshipTypeDataSource
    {
        public static IList<RelationshipType> GetDataSource(UnitOfWorkNoDb uow)
        {
            IList<RelationshipType> existing = uow.GetRepositoryOrNull<RelationshipType>()?.Entities.ToList();
            if (existing != null)
            {
                return existing;
            }

            var list = new List<RelationshipType>();
            list.Add(AddType(uow, "SPOU", "Spouse", ConstituentCategory.Individual, true));
            list.Add(AddType(uow, "FATH", "Father"));
            list.Add(AddType(uow, "MOTH", "Mother"));
            list.Add(AddType(uow, "SON", "Son"));
            list.Add(AddType(uow, "DAU", "Daughter"));
            list.Add(AddType(uow, "BRO", "Brother"));
            list.Add(AddType(uow, "SIS", "Sister"));
            list.Add(AddType(uow, "MEMB", "Member", ConstituentCategory.Both, false, "M"));

            SetReciprocalRelationships(list, "FATH", "SON", "DAU");
            SetReciprocalRelationships(list, "MOTH", "SON", "DAU");
            SetReciprocalRelationships(list, "SON", "FATH", "MOTH");
            SetReciprocalRelationships(list, "DAU", "FATH", "MOTH");
            SetReciprocalRelationships(list, "BRO", "BRO", "SIS");
            SetReciprocalRelationships(list, "SIS", "BRO", "SIS");

            uow.CreateRepositoryForDataSource(list);
            return list;
        }

        private static RelationshipType AddType(IUnitOfWork uow, string code, string name, ConstituentCategory category = ConstituentCategory.Individual, bool isSpouse = false, string categoryCode = "G")
        {
            var type = new RelationshipType()
            {
                Code = code,
                Name = name,
                IsSpouse = isSpouse,
                IsActive = true,
                ConstituentCategory = category,
                Id = Guid.NewGuid()
            };

            if (!string.IsNullOrWhiteSpace(categoryCode))
            {
                type.RelationshipCategory = uow.FirstOrDefault<RelationshipCategory>(p => p.Code == categoryCode);
            }

            return type;

        }

        private static void SetReciprocalRelationships(IList<RelationshipType> list, string code, string recipMale, string recipFemale)
        {
            var type = list.FirstOrDefault(p => p.Code == code);
            if (type != null)
            {
                type.ReciprocalTypeMale = list.FirstOrDefault(p => p.Code == recipMale);
                type.ReciprocalTypeFemale = list.FirstOrDefault(p => p.Code == recipFemale);
            }
        }

    }

    
}
