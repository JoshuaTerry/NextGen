using System;
using System.Collections.Generic;
using System.Linq;

using DDI.Data;
using DDI.Shared;

namespace DDI.Business.Services
{
    public class GenericServiceCommonBase<T> : ServiceBase, IGenericService<T>
        where T : class
    {
        #region Private Fields

        private IRepository<T> _repository;

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

        public IDataResponse<List<T>> GetAll()
        {
            var result = _repository.Entities.ToList();
            return GetIDataResponse(() => result);
        }

        #endregion Public Methods
    }
}
