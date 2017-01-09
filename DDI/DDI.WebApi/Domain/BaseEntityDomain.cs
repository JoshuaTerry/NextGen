using DDI.Data;
using DDI.Shared;
using DDI.Shared.Models;

namespace DDI.WebApi.Domain
{
    public class BaseEntityDomain<T> : IEntityDomain<T> where T : class, IEntity
    {

        public BaseEntityDomain() : this(new Repository<T>())
        {
        }

        internal BaseEntityDomain(IRepository<T> repository)
        {
            this.Repository = repository;
        }

        private IRepository<T> _repository = null;

        /// <summary>
        /// Provides access to repository methods for an entity model.
        /// </summary>
        public IRepository<T> Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = new Repository<T>();
                }
                return _repository;
            }
            set
            {
                _repository = value;
            }
        }

        public virtual void Validate(T entity) { }

    }
}