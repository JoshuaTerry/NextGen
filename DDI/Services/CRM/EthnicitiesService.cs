using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using WebGrease.Css.Extensions;

namespace DDI.Services
{
    public class EthnicitiesService : ServiceBase<Ethnicity>, IEthnicitiesService
    {
        private IConstituentService _constituentService;

        public EthnicitiesService()
            :this(new ConstituentService())
        {
            
        }

        internal EthnicitiesService(IConstituentService constituentService)
        {
            _constituentService = constituentService;
        }

        public IDataResponse AddEthnicitiesToConstituent(Constituent constituent, JObject ethnicityIds)
        {
            var constituentToUpdate = UnitOfWork.GetById<Constituent>(constituent.Id, p => p.Ethnicities);
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
