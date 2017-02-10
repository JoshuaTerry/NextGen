using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using DDI.Data;
using DDI.Shared;
using Newtonsoft.Json.Linq;
using DDI.Business.CRM;
using Microsoft.Ajax.Utilities;
using DDI.Shared.Models.Client.CRM;
using DDI.Services.Search;
using DDI.Shared.Statics;
using DDI.Shared.Logger;

namespace DDI.Services
{
    public class ContactInfoService : ServiceBase<ContactInfo>, IContactInfoService
    {
        private IRepository<ContactInfo> _repository;

        private IUnitOfWork _unitOfWork;

        public ContactInfoService()
        {
            Initialize(new UnitOfWorkEF());
        }
        public ContactInfoService(IUnitOfWork uow)
        {
            Initialize(uow);
        }

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow;
            _repository = _unitOfWork.GetRepository<ContactInfo>();
        }

        public IDataResponse<List<ContactInfo>> GetContactInfoByContactCategoryForConstituent(Guid? categoryId, Guid? constituentId)
        {
            var results = _unitOfWork.GetRepository<ContactInfo>().Entities.Where(c => c.ConstituentId == constituentId && c.ContactType.ContactCategoryId == categoryId).ToList();
            var response = GetIDataResponse(() => results);
            
            response.TotalResults = results.Count;

            return response;
        }
    
    }
}
