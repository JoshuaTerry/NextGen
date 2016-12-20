using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data;
using DDI.Data.Models.Common;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class CountyService : GenericServiceCommonBase<County>
    {
        #region Public Methods

        public IDataResponse<List<County>> GetAll(CountySearch search = null)
        {
            var result = Repository.Entities;
            var query = new CriteriaQuery<County, CountySearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.StateId, d => d.StateId)
                .SetOrderBy(search?.OrderBy);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList());
        }

        #endregion Public Methods

    }
}