using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common;
using System.Collections.Generic;
using System.Linq;
using DDI.Services.Search;
using DDI.Shared.Models;

namespace DDI.Services
{
    public class StateService : ServiceBase<State>
    {
        #region Public Methods

        public override IDataResponse<List<ICanTransmogrify>> GetAll(string fields, IPageable search = null)
        {
            var result = UnitOfWork.GetRepository<State>().GetEntities(IncludesForList);
            var query = new CriteriaQuery<State, ForeignKeySearch>(result, search as ForeignKeySearch)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.Id, d => d.CountryId);

            var response = GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(a => a.DisplayName).ToList<ICanTransmogrify>());
            response.TotalResults = response.Data.Count;

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return response;
        }

        #endregion Public Methods
    }
}
