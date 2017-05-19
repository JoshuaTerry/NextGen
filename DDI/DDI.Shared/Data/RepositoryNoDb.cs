using DDI.Shared;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DDI.Shared.Interfaces;

namespace DDI.Shared.Data
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
    public class RepositoryNoDb<T> : ITestRepository<T>, IRepository
        where T : class
    {
        #region Private Fields
         
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

        IQueryable IRepository.Entities => Entities;

        public ISQLUtilities Utilities => null;

        /// <summary>
        /// A property name or comma delimited list of property names to return via the GetModifiedProperties method.
        /// </summary>
        public string ModifiedPropertyList { get; set; }

        #endregion Public Properties


        #region Public Constructors

        #endregion Public Constructors

        public RepositoryNoDb(IQueryable<T> dataSource)
        {
            Entities = dataSource;
        }

        #region Public Methods

        public EntityState GetEntityState(T entity)
        {
            return (string.IsNullOrWhiteSpace(ModifiedPropertyList) ? EntityState.Unchanged : EntityState.Modified);
        }

        /// <summary>
        /// Clear all entities in the repository.
        /// </summary>
        public void Clear()
        {
            _entities.Clear();
        }

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
        public T Attach(T entity)
        {
            if (entity != null && !_entities.Contains(entity))
            {
                _entities.Add(entity);
            }
            return entity;
        }

        public T Find(params object[] keyValues)
        {
            return GetById(keyValues[0] as Guid? ?? Guid.Empty);
        }

        public T GetById(Guid id)
        {
            return _typedEntities.FirstOrDefault(p => p.Id == (Guid)id) as T;
        }
        

        public virtual T Create()
        {
            T entity = Activator.CreateInstance<T>(); // ...to avoid adding the new() generic type restriction.
            (entity as EntityBase)?.AssignPrimaryKey();
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

        public virtual void UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            UpdateChangedProperties(GetById(id), propertyValues, action);
        }

        public virtual void UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            Type type = entity.GetType();

            foreach (KeyValuePair<string, object> keyValue in propertyValues)
            {
                var propertyInfo = type.GetProperty(keyValue.Key);
                propertyInfo?.SetValue(entity, keyValue.Value);
            }

            action?.Invoke(entity);            
        }

        public List<string> GetModifiedProperties(T entity) 
        {
            if (string.IsNullOrWhiteSpace(ModifiedPropertyList))
            {
                return new List<string>();
            }
            else
            {
                return ModifiedPropertyList.Split(',').ToList();
            }
        }



        public T GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            return GetById(id);
        }

        public IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes)
        {
            return Entities;
        }

        // Note:  The following methods apply only to the RepositoryNoDb class.

        /// <summary>
        /// Assign foreign key Guid properties for all entities.
        /// </summary>
        public void AssignForeignKeys()
        {
            foreach (var entity in _entities)
            {
                AssignForeignKeys(entity);
            }
        }

        /// <summary>
        /// Assign foreign key Guid properties for a specific entity.
        /// </summary>
        public void AssignForeignKeys(T entity)
        {
            var properties = typeof(T).GetProperties().Where(p => p.CanWrite);

            // Step through writeable properties that are Guid or Guid?
            foreach (var prop in properties.Where(p => p.PropertyType == typeof(Guid) || p.PropertyType == typeof(Guid?))) 
            {
                // This is a Guid property. If the name is "xId", look for property "x".
                if (prop.Name.EndsWith("Id"))
                {
                    string entityPropName = prop.Name.Substring(0, prop.Name.Length - 2);
                    var entityProp = properties.FirstOrDefault(p => p.Name == entityPropName);
                    if (entityProp != null)
                    {
                        // Try to get the foreign key value.
                        var fkEntity = entityProp.GetValue(entity) as EntityBase;
                        if (fkEntity != null)
                        {
                            // Set the Id property to the foreign key's Id.
                            prop.SetValue(entity, fkEntity.Id);
                        }
                        else
                        {
                            // Foreign key is null, so set the Id property to null or empty.
                            if (prop.PropertyType == typeof(Guid?))
                            {
                                prop.SetValue(entity, null);
                            }
                            else
                            {
                                prop.SetValue(entity, default(Guid));
                            }
                        }
                    }
                }
            }
        }

        #endregion Public Methods
    }
}
