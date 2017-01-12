using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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
    public class Repository<T> : IRepository<T>
        where T : class
    {
        #region Private Fields

        private readonly DbContext _context = null;
        private IDbSet<T> _entities = null;
        private SQLUtilities _utilities = null;
        private bool _isUOW = false;

        #endregion Private Fields

        #region Public Properties

        public virtual IQueryable<T> Entities => EntitySet;

        public SQLUtilities Utilities
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

        #endregion Public Constructors

        #region Internal Constructors

        public Repository(DbContext context)
        {
            _context = context;
            _isUOW = (context != null);
        }

        #endregion Internal Constructors

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
                if (!_isUOW)
                {
                    _context.SaveChanges();
                }                
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
            try
            {
                // Anything that's not an EF mapped property will throw an exception.

                var reference = _context.Entry(entity).Reference(property);

                if (!reference.IsLoaded)
                    reference.Load();
                return reference.CurrentValue;
            }
            catch(Exception e)
            {
                // Logic to handle BaseLinkedEntity and LinkedEntityCollection:

                // Consult the lambda expression to get the property info.
                if (property.Body is MemberExpression)
                {
                    PropertyInfo propInfo = ((MemberExpression)property.Body).Member as PropertyInfo;
                    if (propInfo != null)
                    {
                        if (entity is BaseLinkedEntity)
                        {
                            // Trying to load a BaseLinkedEntity property.  It's name should be "ParentEntity".
                            if (propInfo.Name == nameof(BaseLinkedEntity.ParentEntity))
                            {
                                // Call the LoadParentEntity method to make sure it's loaded, then return the ParentEntity value.
                                var linkedEntity = entity as BaseLinkedEntity;
                                linkedEntity.LoadParentEntity(_context);
                                return linkedEntity.ParentEntity as TElement;
                            }
                            
                            throw e;  // Wrong property name...
                        }

                        else if (typeof(TElement).GetInterfaces().Contains(typeof(ILinkedEntityCollection)))
                        {
                            // Trying to load a LinkedEntityCollection property.
                            // Get the property value.
                            object memberValue = propInfo.GetValue(entity);

                            if (memberValue == null)
                            {
                                // If null, the LinkedEntityCollection needs to be created.
                                memberValue = (TElement)Activator.CreateInstance(typeof(TElement), entity);
                                propInfo.SetValue(entity, memberValue);
                            }

                            if (memberValue is TElement)
                            {
                                // Ensure the collection is loaded.
                                ((ILinkedEntityCollection)memberValue).LoadCollection(_context);

                                return (TElement)memberValue;
                            }
                        }
                    }
                }

                throw e; // Couldn't determine the reference, so rethrow the exception.
            }
        }

        /// <summary>
        /// Return a collection of entities that have already been loaded or added to the repository.
        /// </summary>
        public ICollection<T> GetLocal()
        {
            return EntitySet.Local;
        }

        /// <summary>
        /// Attach an entity (which may belong to another context) to the repository.
        /// </summary>
        public void Attach(T entity)
        {
            Attach(entity, EntityState.Unchanged);
        }
        
        public T Find(params object[] keyValues) => EntitySet.Find(keyValues);

        public T GetById(object id) => EntitySet.Find(id);

        public virtual T Create()
        {
            T entity = Activator.CreateInstance<T>(); // ...to avoid adding the new() generic type restriction.
            (entity as BaseEntity)?.AssignPrimaryKey();
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
                    // Add it only if not already added.
                    EntitySet.Add(entity);
                }

                if (!_isUOW)
                {
                    _context.SaveChanges();
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

                Attach(entity, EntityState.Modified);
                _context.Entry(entity).State = EntityState.Modified;
                if (!_isUOW)
                {
                    _context.SaveChanges();
                }

                return entity;
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.GetFriendlyMessage(), e);
            }
        }

        public virtual int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            return UpdateChangedProperties(GetById(id), propertyValues, action);
        }

        public virtual int UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            DbEntityEntry<T> entry = _context.Entry(entity);
            DbPropertyValues currentValues = entry.CurrentValues;

            foreach (KeyValuePair<string, object> keyValue in propertyValues)
            {
                currentValues[keyValue.Key] = keyValue.Value;
            }

            action?.Invoke(entity);

            return _isUOW ? 0 : _context.SaveChanges();
        }

        public List<string> GetModifiedProperties(T entity) 
        {
            var list = new List<string>();

            var entry = _context.Entry(entity);
            foreach (var property in entry.OriginalValues.PropertyNames)
            {
                if (entry.Property(property).IsModified)
                {
                    list.Add(property);
                }
            }

            return list;
        }

        #endregion Public Methods

        #region Private Methods

        private void Attach(T entity, EntityState entityState)
        {
            if (entity != null && _context.Entry(entity).State == EntityState.Detached)
            {
                // Attach throws exceptions if parts of the entity graph are already in the context.  Instead, use Add and adjust the entity state.
                EntitySet.Add(entity);
                _context.Entry(entity).State = entityState;
            }
        }

        #endregion

    }
}
