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

        public override IDataResponse<List<State>> GetAll(IPageable search = null)
        {
            var result = UnitOfWork.GetRepository<State>().GetEntities(IncludesForList);
            var query = new CriteriaQuery<State, ForeignKeySearch>(result, search as ForeignKeySearch)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.Id, d => d.CountryId);

            var response = GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(a => a.DisplayName).ToList());
            response.TotalResults = response.Data.Count;

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return response;
        }

        #endregion Public Methods
    }
}
