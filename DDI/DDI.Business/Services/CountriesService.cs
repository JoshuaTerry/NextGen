using DDI.Data;
using DDI.Data.Models.Common;
using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class CountriesService : ServiceBase, IGenericService<Country>
    {
        private IRepository<Country> _repository;
        public CountriesService(): this(new Repository<Country>()) { }
        internal CountriesService(IRepository<Country> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<Country>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    } 
}