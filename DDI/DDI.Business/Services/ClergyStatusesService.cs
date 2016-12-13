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
    public class ClergyStatusesService : ServiceBase, IGenericService<ClergyStatus>
    {
        private IRepository<ClergyStatus> _repository;
        public ClergyStatusesService() : this(new Repository<ClergyStatus>()) { }
        internal ClergyStatusesService(IRepository<ClergyStatus> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<ClergyStatus>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}