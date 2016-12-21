using System.Collections.Generic;
using System.Linq;
using DDI.Shared;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data.Models.Common;
using DDI.Data;

namespace DDI.Business.Services
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
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m=> m.CountryId, d => d.CountryId)
                .SetOrderBy(search?.OrderBy);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList());
        }

        #endregion Public Methods
    }
}
