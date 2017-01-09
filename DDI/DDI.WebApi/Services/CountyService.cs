using System.Collections.Generic;
using System.Linq;
using DDI.Data.Models.Common;
using DDI.Shared;
using DDI.WebApi.Helpers;
using Services.Search;

namespace DDI.WebApi.Services
{
    public class CountyService : GenericServiceCommonBase<County>
    {
        #region Public Methods

        public IDataResponse<List<County>> GetAll(CountySearch search = null)
        {
            var result = Repository.Entities;
            var query = new CriteriaQuery<County, CountySearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.StateId, d => d.StateId);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(c => c.DisplayName).ToList());
        }

        #endregion Public Methods

    }
}