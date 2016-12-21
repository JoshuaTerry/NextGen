using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Data.Models.Common;
using DDI.Shared;
using DDI.WebApi.Helpers;
using DDI.WebApi.Services.Search;

namespace DDI.WebApi.Services
{
    public class StateService : GenericServiceCommonBase<State>
    {
        public StateService() : this(new CachedRepository<State>(new CommonContext()))
        {
        }

        internal StateService(IRepository<State> repository) : base(repository)
        { 
        }
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
