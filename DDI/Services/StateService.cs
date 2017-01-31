using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common; 
using System.Collections.Generic;
using System.Linq;
using DDI.Services.Search;

namespace DDI.Services
{
    public class StateService : ServiceBase<State>
    {
        #region Public Methods

        public IDataResponse<List<State>> GetAll(ForeignKeySearch search= null)
        {
            var result = UnitOfWork.GetRepository<State>().Entities;
            var query = new CriteriaQuery<State, ForeignKeySearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.Id, d => d.CountryId);

            var response = GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(a => a.DisplayName).ToList());
            response.TotalResults = response.Data.Count;

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return response;
        }

        #endregion Public Methods
    }
}
