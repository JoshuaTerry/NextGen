﻿using DDI.Services.Search;
using DDI.Shared;
using DDI.Shared.Models.Common;
using System.Collections.Generic;
using System.Linq;

namespace DDI.Services
{
    public class CountyService : ServiceBase<County>
    {
        #region Public Methods

        public IDataResponse<List<County>> GetAll(ForeignKeySearch search = null)
        {
            var result = UnitOfWork.GetRepository<County>().Entities;
            var query = new CriteriaQuery<County, ForeignKeySearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.Id, d => d.StateId);

            var response = GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(c => c.DisplayName).ToList());
            response.TotalResults = response.Data.Count;

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return response;
        }

        #endregion Public Methods

    }
}
