using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;

using DDI.Data;
using DDI.Shared;
using DDI.Business.Helpers;
using DDI.Business.Services.Search;
using DDI.Data.Models;
using DDI.Data.Models.Common;

namespace DDI.Business.Services
{
    public class GenericServiceCommonBase<T> : ServiceBase, IGenericService<T>
        where T : class, IEntity
    {
        #region Private Fields

        private IRepository<T> _repository;
        public IRepository<T> Repository => _repository;

        #endregion Private Fields

        #region Public Constructors

        public GenericServiceCommonBase()
            : this(new Repository<T>(new CommonContext()))
        {
        }

        #endregion Public Constructors

        #region Internal Constructors

        internal GenericServiceCommonBase(IRepository<T> repository)
        {
            _repository = repository;
        }

        #endregion Internal Constructors

        #region Public Methods

        public IDataResponse<List<T>> GetAll(IPageable search= null)
        {
            var result = _repository.Entities.ToList().OrderBy(a => a.DisplayName).ToList();
            return GetIDataResponse(() => result);
        }

        #endregion Public Methods
    }
}
