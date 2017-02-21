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
    public class DenominationsService : ServiceBase<Denomination>, IDenominationsService
    {
        private IConstituentService _constituentService;

        public DenominationsService()
            :this(new ConstituentService())
        {

        }

        internal DenominationsService(IConstituentService constituentService)
        {
            _constituentService = constituentService;
        }

        public IDataResponse AddDenominationsToConstituent(Constituent constituent, JObject denominationIds)
        {
            var constituentToUpdate = UnitOfWork.GetById<Constituent>(constituent.Id, c => c.Denominations);
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
