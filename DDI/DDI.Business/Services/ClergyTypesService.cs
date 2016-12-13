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
    public class ClergyTypesService : ServiceBase, IGenericService<ClergyType>
    {
        private IRepository<ClergyType> _repository;
        public ClergyTypesService() : this(new Repository<ClergyType>()) { }
        internal ClergyTypesService(IRepository<ClergyType> repository)
        {
            _repository = repository;
        }
        public IDataResponse<List<ClergyType>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }
    }
}