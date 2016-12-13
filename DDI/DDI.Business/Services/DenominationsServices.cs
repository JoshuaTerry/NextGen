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
    public class DenominationsServices : ServiceBase, IGenericService<Denomination>
    {
        private IRepository<Denomination> _repository;
        public DenominationsServices(): this(new Repository<Denomination>()) { }
        internal DenominationsServices(IRepository<Denomination> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<Denomination>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}