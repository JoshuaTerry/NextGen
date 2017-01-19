using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.CRM;

namespace DDI.Business.Tests.CRM.DataSources
{
    public static class GenderDataSource
    {
        public static IList<Gender> GetDataSource(UnitOfWorkNoDb uow)
        {
            var _genders = new List<Gender>();
            _genders.Add(new Gender() { Code = "M", Name = "Male", IsMasculine = true, Id = Guid.NewGuid() });
            _genders.Add(new Gender() { Code = "F", Name = "Female", IsMasculine = false, Id = Guid.NewGuid() });

            uow.CreateRepositoryForDataSource(_genders);
            return _genders;
        }    

    }

    
}
