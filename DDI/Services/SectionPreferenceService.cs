﻿using System.Collections.Generic;
using System.Linq;
using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models.Client.Core;
using DDI.Services;

namespace DDI.Services
{
    public class SectionPreferenceService : ServiceBase<SectionPreference>, ISectionPreferenceService
    {
        private IRepository<SectionPreference> _repository;

        private IUnitOfWork _unitOfWork;

        public SectionPreferenceService()
        {
            Initialize(new UnitOfWorkEF());
        }
        public SectionPreferenceService(IUnitOfWork uow)
        {
            Initialize(uow);
        }

        private void Initialize(IUnitOfWork uow)
        {
            _unitOfWork = uow; 
            _repository = _unitOfWork.GetRepository<SectionPreference>();
        }
        public IDataResponse<List<SectionPreference>> GetPreferencesBySectionName(string sectionName)
        {
            var results = _unitOfWork.GetRepository<SectionPreference>().Entities.Where(p => p.SectionName == sectionName).ToList();
            return GetIDataResponse(() => results);
        }
    }
}