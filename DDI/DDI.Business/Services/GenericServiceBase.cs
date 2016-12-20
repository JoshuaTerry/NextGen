using DDI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class GenericServiceBase<T> : ServiceBase, IGenericService<T> where T : class
    {
        private IRepository<T> _repository;
        public GenericServiceBase(): this(new Repository<T>()) { }
        public GenericServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<T>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
        
    }
}