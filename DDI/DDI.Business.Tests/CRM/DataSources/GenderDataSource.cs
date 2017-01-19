﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Business.Tests.Helpers;
using DDI.Data;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class GenderDataSource
    {
        public static IList<Gender> GetDataSource(UnitOfWorkNoDb uow)
        {
            var genders = new List<Gender>();
            genders.Add(new Gender() { Code = "M", Name = "Male", IsMasculine = true, Id = GuidHelper.NextGuid() });
            genders.Add(new Gender() { Code = "F", Name = "Female", IsMasculine = false, Id = GuidHelper.NextGuid() });

            uow.CreateRepositoryForDataSource(genders);
            return genders;
        }    

    }

    
}
