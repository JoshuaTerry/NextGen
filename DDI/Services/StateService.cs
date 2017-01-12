using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Common; 
using System.Collections.Generic;
using System.Linq;
using DDI.Services.Search;

namespace DDI.Services
{
    public class StateService : ServiceBase<County>
    {
        public StateService() : base(new UnitOfWorkEF(new CommonContext()))
        {
        }

        internal StateService(IUnitOfWork uow) : base(uow)
        { 
        }
        #region Public Methods

        public IDataResponse<dynamic> GetAll(StateSearch search= null)
        {
            var result = UnitOfWork.GetRepository<State>().Entities;
            var query = new CriteriaQuery<State, StateSearch>(result, search)
                .IfModelPropertyIsNotBlankAndItEqualsDatabaseField(m => m.CountryId, d => d.CountryId);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList().OrderBy(a => a.DisplayName).ToList());
        }

        #endregion Public Methods
    }
}
