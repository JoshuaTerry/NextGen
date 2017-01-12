using System;
using System.Collections;
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

        #endregion Public Constructors

        #region Public Methods

        public static string NameFor<T>(Expression<Func<T, object>> property, bool shouldContainObjectPath = false)
        {
            var member = property.Body as MemberExpression;
            if (member == null)
            {
                var unary = property.Body as UnaryExpression;
                if (unary != null)
                {
                    member = unary.Operand as MemberExpression;
                }
            }
            if (shouldContainObjectPath && member != null)
            {
                var path = member.Expression.ToString();
                var objectPath = member.Expression.ToString().Split('.');
                if (objectPath.Length >= 2)
                {
                    path = String.Join(".", objectPath, 1, objectPath.Length - 1);
                    return $"{path}.{member.Member.Name}";
                }
            }
            return member?.Member.Name ?? String.Empty;
        }

        public virtual void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    EntitySet.Attach(entity);
                }

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
            if (entity != null)
            {
                EntitySet.Attach(entity);
            }
        }
        
        public T Find(params object[] keyValues) => EntitySet.Find(keyValues);

        public IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes)
        {
            if (includes == null || includes.Length == 0)
            {
                return Entities;
            }

            var query = _context.Set<T>().AsQueryable();

            foreach(Expression<Func<T, object>> include in includes)
            {
                string name = NameFor(include, true);
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

        public List<string> GetModifiedProperties(T entity)
        {
            var list = new List<string>();
            DbEntityEntry<T> entry = _context.Entry(entity);

            foreach (string property in entry.OriginalValues.PropertyNames)
            {
                if (entry.Property(property).IsModified)
                {
                    list.Add(property);
                }
            }

            return list;
        }

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

                EntitySet.Attach(entity);
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

        #endregion Public Methods
    }
}
