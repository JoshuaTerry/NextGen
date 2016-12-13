using DDI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DDI.Business.Services
{
    public class GenericServiceBase<T> : ServiceBase where T : class
    {
        private IRepository<T> _repository;
        public GenericServiceBase(): this(new Repository<T>()) { }
        internal GenericServiceBase(IRepository<T> repository)
        {
            _repository = repository;
        }
    }
}