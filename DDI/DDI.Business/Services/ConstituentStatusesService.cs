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
    public class ConstituentStatusesService : ServiceBase, IGenericService<ConstituentStatus>
    {
        private IRepository<ConstituentStatus> _repository;
        public ConstituentStatusesService(): this(new Repository<ConstituentStatus>()) { }
        internal ConstituentStatusesService(IRepository<ConstituentStatus> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<ConstituentStatus>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}