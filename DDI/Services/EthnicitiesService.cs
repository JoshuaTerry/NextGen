using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Services.ServiceInterfaces;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;
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

        public IDataResponse<Constituent> AddEthnicitiesToConstituent(Constituent constituent, JObject ethnicityIds)
        {
            var constituentRepo = UnitOfWork.GetRepository<Constituent>();
            var constituentToUpdate = constituentRepo.Entities.Include("Ethnicities").SingleOrDefault(c => c.Id == constituent.Id);
            IDataResponse<Constituent> response = null;
            List<Ethnicity> passedEthnicities = new List<Ethnicity>();
            List<Ethnicity> constituentEthnicities = new List<Ethnicity>();

            foreach (var pair in ethnicityIds)
            {
                if (pair.Value.Type == JTokenType.Array && pair.Value.HasValues)
                {
                    passedEthnicities.AddRange(from jToken in (JArray)pair.Value select Guid.Parse(jToken.ToString()) into id select base.GetById(id).Data);
                }
            }

            constituentEthnicities = constituentRepo.Entities.Single(c => c.Id == constituentToUpdate.Id).Ethnicities.ToList();

            var removes = constituentEthnicities.Except(passedEthnicities);
            var adds = passedEthnicities.Except(constituentEthnicities);

            if (constituentToUpdate != null)
            {
                removes.ForEach(e => constituentToUpdate.Ethnicities.Remove(e));
                adds.ForEach(e => constituentToUpdate.Ethnicities.Add(e));
            }

            UnitOfWork.SaveChanges();

            response = GetIDataResponse(() => constituentToUpdate);

            return response;
        }
    }
}
