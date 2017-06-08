using System;
using System.Collections.Generic;
using System.Linq;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Extensions;
using DDI.Shared.Models.Client.CRM;
using DDI.Shared.Statics.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services
{
    public class DenominationsService : ServiceBase<Denomination>, IDenominationsService
    {
        public DenominationsService(IUnitOfWork uow) : base(uow) { }

        public IDataResponse AddDenominationsToConstituent(Guid constituentId, JObject denominationIds)
        {

            var constituentToUpdate = UnitOfWork.GetById<Constituent>(constituentId, c => c.Denominations);
            if (constituentToUpdate == null)
            {
                return GetErrorResponse(UserMessagesCRM.ConstituentIdInvalid);
            }

            IDataResponse response = null;
            List<Denomination> passedDenominations = new List<Denomination>();
            List<Denomination> constituentDenominations = new List<Denomination>();

            foreach (var pair in denominationIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passedDenominations.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select base.GetById(id).Data);
                }
            }

            constituentDenominations = constituentToUpdate.Denominations.ToList();

            var removes = constituentDenominations.Except(passedDenominations);
            var adds = passedDenominations.Except(constituentDenominations);

            if (constituentToUpdate != null)
            {
                removes.ForEach(e => constituentToUpdate.Denominations.Remove(e));
                adds.ForEach(e => constituentToUpdate.Denominations.Add(e));
            }

            UnitOfWork.SaveChanges();

            response = new DataResponse()
            {
                IsSuccessful = true
            };

            return response;
        }
    }
}
