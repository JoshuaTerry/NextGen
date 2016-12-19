using DDI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Business.Helpers;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class GenericServiceBase<T> : ServiceBase, IGenericService<T> where T : class
    {
        private IRepository<T> _repository;
        public GenericServiceBase(): this(new Repository<T>()) { }
        internal GenericServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<T>> GetAll(string orderBy = "Name")
        {
            var search = new PageableSearch
            {
                OrderBy = orderBy //nameof(IEntity.DisplayName)
            };
            var result = _repository.Entities;
            var query = new CriteriaQuery<T, PageableSearch>(result, search)
                .SetOrderBy(search.OrderBy);

            //var sql = query.GetQueryable().ToString();  //This shows the SQL that is generated
            return GetIDataResponse(() => query.GetQueryable().ToList());
        }

    }
}