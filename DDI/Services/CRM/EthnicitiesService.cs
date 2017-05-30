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
    public class EthnicitiesService : ServiceBase<Ethnicity>, IEthnicitiesService
    {
        public EthnicitiesService(IUnitOfWork uow) : base(uow) { }

        public IDataResponse AddEthnicitiesToConstituent(Guid constituentId, JObject ethnicityIds)
        {
            var constituentToUpdate = UnitOfWork.GetById<Constituent>(constituentId, p => p.Ethnicities);
            if (constituentToUpdate == null)
            {
                return GetErrorResponse(UserMessagesCRM.ConstituentIdInvalid);
            }

            IDataResponse response = null;
            List<Ethnicity> passedEthnicities = new List<Ethnicity>();
            List<Ethnicity> constituentEthnicities = new List<Ethnicity>();

            foreach (var pair in ethnicityIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passedEthnicities.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select base.GetById(id).Data);
                }
            }

            constituentEthnicities = constituentToUpdate.Ethnicities.ToList();

            var removes = constituentEthnicities.Except(passedEthnicities);
            var adds = passedEthnicities.Except(constituentEthnicities);

            if (constituentToUpdate != null)
            {
                removes.ForEach(e => constituentToUpdate.Ethnicities.Remove(e));
                adds.ForEach(e => constituentToUpdate.Ethnicities.Add(e));
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
