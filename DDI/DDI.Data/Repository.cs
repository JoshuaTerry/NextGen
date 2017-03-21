using DDI.Data.Helpers;
using DDI.Logger;
using DDI.Shared;
using DDI.Shared.Helpers;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

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
    public class Repository<T> : IRepository<T>, IRepository
        where T : class
    {
        #region Private Fields

        private readonly DbContext _context = null;
        private IDbSet<T> _entities = null;
        private SQLUtilities _utilities = null;
        private bool _isUOW = false;
        private ICollection<T> _local = null;
        private readonly ILogger _logger = LoggerManager.GetLogger(typeof(Repository<T>));
        #endregion Private Fields

        #region Public Properties
        protected ILogger Logger => _logger;

        public virtual IQueryable<T> Entities => EntitySet;

        IQueryable IRepository.Entities => EntitySet;

        public ISQLUtilities Utilities
        {
            get
            {
                if (_utilities == null && _context != null)
                {
                    _utilities = new SQLUtilities(_context);
                }

                return _utilities;
            }
        }

        #endregion Public Properties

        #region Protected Properties

        protected IDbSet<T> EntitySet
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }

                return _entities;
            }
        }

        #endregion Protected Properties

        #region Public Constructors

        public Repository() :
            this(new DomainContext())
        {
            _isUOW = false;
        }

        public Repository(DbContext context)
        {
            _context = context;
            _isUOW = (context != null);
        }
        #endregion Public Constructors

        #region Public Methods       

        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                Attach(entity);

                EntitySet.Remove(entity);
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.GetFriendlyMessage(), e);
            }
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
            var entryCollection = _context.Entry(entity).Collection(collection);
            if (!entryCollection.IsLoaded)
                entryCollection.Load();
            return entryCollection.CurrentValue;
        }

        /// <summary>
        /// Explicitly load a reference property or collection for an entity and return the value.
        /// </summary>
        public TElement GetReference<TElement>(T entity, System.Linq.Expressions.Expression<Func<T, TElement>> property) where TElement : class
        {
            var reference = _context.Entry(entity).Reference(property);

            if (!reference.IsLoaded)
                reference.Load();
            return reference.CurrentValue;
        }

        /// <summary>
        /// Return a collection of entities that have already been loaded or added to the repository.
        /// </summary>
        public ICollection<T> GetLocal()
        {
            if (_local == null)
            {
                _local = EntitySet.Local;
            }
            return _local;
        }

        /// <summary>
        /// Attach an entity (which may belong to another context) to the repository.
        /// </summary>
        public T Attach(T entity)
        {
            return Attach(entity, EntityState.Unchanged);
        }

        public T Find(params object[] keyValues) => EntitySet.Find(keyValues);

        public IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return Entities;
            }

            var query = _context.Set<T>().AsQueryable();

            foreach (Expression<Func<T, object>> include in includes)
            {
                string name = PathHelper.NameFor(include, true);
                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Include(name);
                }
            }

            return query;
        }

        public T GetById(Guid id) => EntitySet.Find(id);

        public T GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            if (typeof(IEntity).IsAssignableFrom(typeof(T)))
            {
                var query = (IQueryable<IEntity>)GetEntities(includes);
                return query.FirstOrDefault(p => p.Id == id) as T;
            }
            else
            {
                return GetById(id);
            }
        }

        public virtual T Create()
        {
            T entity = Activator.CreateInstance<T>(); // ...to avoid adding the new() generic type restriction.
            (entity as EntityBase)?.AssignPrimaryKey();
            EntitySet.Add(entity);

            return entity;
        }

        public virtual T Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (_context.Entry(entity).State != EntityState.Added)
                {
                    IAuditableEntity auditableEntity = entity as IAuditableEntity;
                    if (auditableEntity != null)
                    {
                        var user = EntityFrameworkHelpers.GetCurrentUser();
                        if (user != null)
                        {
                            auditableEntity.CreatedBy = user.UserName;
                            auditableEntity.LastModifiedBy = user.UserName;
                        }

                        auditableEntity.CreatedOn = DateTime.UtcNow;
                        auditableEntity.LastModifiedOn = DateTime.UtcNow;
                    }
                    // Add it only if not already added.
                    EntitySet.Add(entity);
                }

                return entity;
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.GetFriendlyMessage(), e);
            }
        }

        public virtual T Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                IAuditableEntity auditableEntity = entity as IAuditableEntity;
                if (auditableEntity != null)
                {
                    var user = EntityFrameworkHelpers.GetCurrentUser();
                    if (user != null)
                    {
                        auditableEntity.LastModifiedBy = user.UserName;
                    }

                    auditableEntity.LastModifiedOn = DateTime.UtcNow;
                }
                Attach(entity, EntityState.Modified);
                _context.Entry(entity).State = EntityState.Modified;

                return entity;
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.GetFriendlyMessage(), e);
            }
        }

        public virtual void UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            UpdateChangedProperties(GetById(id), propertyValues, action);
        }

        public virtual void UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            DbEntityEntry<T> entry = _context.Entry(entity);
            DbPropertyValues currentValues = entry.CurrentValues;
            IEnumerable<string> propertynames = currentValues.PropertyNames;
            foreach (KeyValuePair<string, object> keyValue in propertyValues)
            {
                if (propertynames.Contains(keyValue.Key))
                {
                    currentValues[keyValue.Key] = keyValue.Value;
                }
                else
                {
                    // NotMapped property: Use reflection to try and set the property in the entity.
                    typeof(T).GetProperty(keyValue.Key)?.SetValue(entity, keyValue.Value);
                }
            }

            action?.Invoke(entity);
        }
        public List<string> GetModifiedProperties(T entity)
        {
            var list = new List<string>();
            DbEntityEntry<T> entry = _context.Entry(entity);

            if (entry.State != EntityState.Detached)
            {
                foreach (string property in entry.OriginalValues.PropertyNames)
                {
                    if (entry.Property(property).IsModified)
                    {
                        list.Add(property);
                    }
                }
            }

            return list;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Attach an entity to the context.  If it's already loaded, return the loaded entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityState"></param>
        /// <returns></returns>
        private T Attach(T entity, EntityState entityState)
        {
            if (entity != null && _context.Entry(entity).State == EntityState.Detached)
            {
                if (entity is IEntity)
                {
                    // Get the entity's Id and look for an already loaded instance in EntitySet.Local.  If found, return the loaded entity.
                    Guid id = ((IEntity)entity).Id;
                    IEntity existing = EntitySet.Local.Cast<IEntity>().FirstOrDefault(p => p.Id == id);
                    if (existing != null)
                    {
                        return existing as T;
                    }
                }
            }

            // Attach throws exceptions if parts of the entity graph are already in the context.  Instead, use Add and adjust the entity state.
            EntitySet.Add(entity);
            _context.Entry(entity).State = entityState;
            return entity;
        }

        #endregion

    }
}
