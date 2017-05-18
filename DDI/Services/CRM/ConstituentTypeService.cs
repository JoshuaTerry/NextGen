using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class ConstituentTypeService : ServiceBase<ConstituentType>, IConstituentTypeService
    {
        #region Private Fields

        #endregion

        #region Public Methods



        public IDataResponse AddTagsToConstituentType(ConstituentType constituentType, JObject tagIds)
        {
            var constituentTypeToUpdate = UnitOfWork.GetById<ConstituentType>(constituentType.Id, c => c.Tags);
            IDataResponse response = null;
            List<Tag> passedTags = new List<Tag>();
            List<Tag> constituentTypeTags = new List<Tag>();

            foreach (var pair in tagIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passedTags.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select UnitOfWork.GetById<Tag>(id));
                }
            }

            constituentTypeTags = constituentTypeToUpdate.Tags.ToList();

            var removes = constituentTypeTags.Except(passedTags);
            var adds = passedTags.Except(constituentTypeTags);

            if (constituentTypeToUpdate != null)
            {
                removes.ForEach(r => constituentTypeToUpdate.Tags.Remove(r));
                adds.ForEach(a => constituentTypeToUpdate.Tags.Add(a));
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse<ConstituentType>()
            {
                Data = UnitOfWork.GetById<ConstituentType>(constituentType.Id),
                IsSuccessful = true

            };

            return response;
        }

        public IDataResponse RemoveTagFromConstituentType(ConstituentType constituentType, Guid tagId)
        {
            IDataResponse response = null;
            var tagToRemove = constituentType.Tags.Where(t => t.Id == tagId).FirstOrDefault();

            if (tagToRemove != null)
            {
                constituentType.Tags.Remove(tagToRemove);
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse<ConstituentType>()
            {
                Data = UnitOfWork.GetById<ConstituentType>(constituentType.Id),
                IsSuccessful = true

            };

            return response;
        }

        #endregion

        #region Private Methods



        #endregion
    }
}
