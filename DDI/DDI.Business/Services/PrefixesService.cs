using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Data.Models.Common;
using DDI.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class PrefixesService : ServiceBase, IGenericService<Prefix>
    {
        private IRepository<Prefix> _repository;
        public PrefixesService(): this(new Repository<Prefix>()) { }
        internal PrefixesService(IRepository<Prefix> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<Prefix>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}