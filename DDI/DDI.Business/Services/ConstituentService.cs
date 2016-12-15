﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using DDI.Business.Helpers;
using DDI.Data;
using DDI.Data.Models.Client;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class ConstituentService : ServiceBase, IConstituentService
    {
        private IRepository<Constituent> _repository; 

        public ConstituentService():
            this(new Repository<Constituent>())
        {
            
        }

        internal ConstituentService(IRepository<Constituent> repository)
        {
            _repository = repository;
        }

        public IDataResponse<List<Constituent>> GetConstituents(ConstituentSearch search)
        {
            IQueryable<Constituent> constituents = _repository.Entities;
            var query = new CriteriaQuery<Constituent, ConstituentSearch>(constituents, search)
                .IfContains(m => m.Name, c => c.FormattedName);
            //            constituents = constituents.Where(c => (c.FirstName.Contains(search.Name) || c.LastName.Contains(search.Name)));
            //            constituents = constituents.OrderBy(c => c.LastName);

            return GetIDataResponse(() => query.GetQueryable());

            var pageSize = (search.Limit ?? 100);
            if ((search.Offset ?? 0) > 0)
            {
                constituents = constituents.Skip(search.Offset.Value * pageSize);
            }
            constituents = constituents.Take(pageSize);

            return GetIDataResponse(() => constituents.ToList());
        }

        public IDataResponse<Constituent> GetConstituentById(Guid id)
        {
            Constituent constituent = _repository.GetById(id);

            return GetIDataResponse(() => constituent);
        }

        public IDataResponse<Constituent> GetConstituentByConstituentNum(int constituentNum)
        {
            var constituent = _repository.Entities.FirstOrDefault(c => c.ConstituentNumber == constituentNum);

            return GetIDataResponse(() => constituent);
        }
    }
}
