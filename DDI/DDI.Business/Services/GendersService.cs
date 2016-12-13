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
    public class GendersService : ServiceBase, IGenericService<Gender>
    {
        private IRepository<Gender> _repository;
        public GendersService(): this(new Repository<Gender>()) { }
        internal GendersService(IRepository<Gender> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<Gender>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}