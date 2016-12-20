using DDI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data.Models;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class GenericServiceBase<T> : ServiceBase, IGenericService<T> where T : class, IEntity
    {
        private IRepository<T> _repository;
        public GenericServiceBase(): this(new Repository<T>()) { }
        public GenericServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<T>> GetAll(IPageable search = null)
        {
            var result = _repository.Entities.ToList().OrderBy(a => a.DisplayName).ToList();
            return GetIDataResponse(() => result);
        }

    }
}