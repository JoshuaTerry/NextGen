using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Shared.Enums.Common;
using DDI.Services;

namespace DDI.Services
{
    public class CustomFieldService : ServiceBase<CustomField>, ICustomFieldService
    {
        private IRepository<CustomField> _repository;

        private IUnitOfWork _unitOfWork;

        public CustomFieldService()
        {
            Initialize(new UnitOfWorkEF());
        }
        public CustomFieldService(IUnitOfWork uow)
        {
            Initialize(uow);
        }

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _repository = _unitOfWork.GetRepository<CustomField>();
        }

        public IDataResponse<List<CustomField>> GetByEntityId(int entityId)
        {
            var results = _unitOfWork.GetRepository<CustomField>().Entities.Where(c => c.Entity == (CustomFieldEntity)entityId).ToList();
            return GetIDataResponse(() => results);
        }
    }
}
