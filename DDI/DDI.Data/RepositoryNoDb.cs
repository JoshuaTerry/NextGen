using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models;

namespace DDI.Data
{
    /// <summary>
    /// Base Entity Framework repository implementation that provides a set of common data access
    /// operations for all entities.
    /// </summary>
    /// <remarks>
    /// An entity does not require its own repository class unless it requires additional
    /// functionality that is not covered by the basic add, update, delete, list operations that this
    /// class provides.
    /// </remarks>
    public class RepositoryNoDb<T> : IRepository<T>
        where T : class
    {
        #region Private Fields

        private bool _isUOW = false;
        private IList<T> _entities = null;
        private IEnumerable<IEntity> _typedEntities = null;

        #endregion Private Fields

        #region Public Properties

        public virtual IQueryable<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = new List<T>();
                    _typedEntities = new List<IEntity>();
                }

                return _entities.AsQueryable();
            }
            set
            {
                _entities = value.AsEnumerable().ToList();

                if (typeof(IEntity).IsAssignableFrom(typeof(T)))
                {
                    _typedEntities = _entities.Cast<IEntity>();
                }
                else
                {
                    _typedEntities = new List<IEntity>();
                }
            }
        }

        public SQLUtilities Utilities
        {
            get
            {
                return null;
            }
        }

        #endregion Public Properties


        #region Public Constructors

        #endregion Public Constructors

        public RepositoryNoDb(IQueryable<T> dataSource)
        {
            Entities = dataSource;
        }

        #region Public Methods



        public virtual void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Remove(entity);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity.
        /// </summary>
        public void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class
        {
            GetReference<TElement>(entity, collection);
        }
        
        /// <summary>
        /// Explicitly load a reference property or collection for an entity.
        /// </summary>
        public void LoadReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class
        {
            GetReference<TElement>(entity, property);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity and return the value.
        /// </summary>
        public ICollection<TElement> GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, ICollection<TElement>>> collection) where TElement : class
        {
            var method = collection.Compile();
            return method.Invoke(entity);
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity and return the value.
        /// </summary>
        public TElement GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class
        {
            var method = property.Compile();
            return method.Invoke(entity);
        }

        /// <summary>
        /// Return a collection of entities that have already been loaded or added to the repository.
        /// </summary>
        public ICollection<T> GetLocal()
        {
            return _entities;
        }

        /// <summary>
        /// Attach an entity (which may belong to another context) to the repository.
        /// </summary>
        public void Attach(T entity)
        {
            if (entity != null && !_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
        }

        public T Find(params object[] keyValues)
        {
            return GetById(keyValues[0]);
        }

        public T GetById(object id)
        {
            if (id is Guid)
            {
                return _typedEntities.FirstOrDefault(p => p.Id == (Guid)id) as T;
            }
            return null;
        }

        public virtual T Create()
        {
            T entity = Activator.CreateInstance<T>(); // ...to avoid adding the new() generic type restriction.
            (entity as BaseEntity)?.AssignPrimaryKey();
            _entities.Add(entity);

            return entity;
        }

        public virtual T Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Attach(entity);

            return entity;
        }

        public virtual T Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            Attach(entity);

            return entity;
        }

        public virtual int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            return UpdateChangedProperties(GetById(id), propertyValues, action);
        }

        public virtual int UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            Type type = entity.GetType();

            foreach (KeyValuePair<string, object> keyValue in propertyValues)
            {
                var propertyInfo = type.GetProperty(keyValue.Key);
                propertyInfo?.SetValue(entity, keyValue.Value);
            }

            action?.Invoke(entity);

            return 1;
        }

        public List<string> GetModifiedProperties(T entity) 
        {
            return new List<string>();
        }        

        #endregion Public Methods
    }
}
