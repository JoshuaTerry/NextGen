﻿using DDI.Shared;
using DDI.Shared.Models.Client.CRM;
using Newtonsoft.Json.Linq;

namespace DDI.Services.ServiceInterfaces
{
    public interface IEthnicitiesService : IService<Ethnicity>
    {
        IDataResponse AddEthnicitiesToConstituent(Constituent constituent, JObject ethnicityIds);
    }
}