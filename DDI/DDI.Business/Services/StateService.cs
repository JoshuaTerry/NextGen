using System;
using System.Collections.Generic;
using System.Linq;

using DDI.Data;
using DDI.Shared;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data.Models;
using DDI.Data.Models.Common;

namespace DDI.Business.Services
{
    public class StateService: GenericServiceCommonBase<State>
    {

        #region Public Methods

        public IDataResponse<List<State>> GetAll(StateSearch search= null)
        {
            var result = Repository.Entities;
            var query = new CriteriaQuery<State, StateSearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.CountryId, d => d.CountryId);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(a => a.DisplayName).ToList());
        }

        #endregion Public Methods
    }
}
