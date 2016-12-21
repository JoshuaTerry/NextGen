using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion Private Fields

        #region Public Properties

        public virtual IQueryable<T> Entities => EntitySet;

        public SQLUtilities Utilities
        {
            get
            {
                if (_utilities == null)
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
        }

        #endregion Public Constructors

        #region Internal Constructors

        public Repository(DbContext context)
        {
            _context = context;
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

                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    EntitySet.Attach(entity);
                }

                EntitySet.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw new Exception(e.GetFriendlyMessage(), e);
            }
        }

        public T Find(params object[] keyValues) => EntitySet.Find(keyValues);

        public T GetById(object id) => EntitySet.Find(id);

        public virtual T Insert(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException(nameof(entity));
                }

                EntitySet.Add(entity);
                _context.SaveChanges();

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
                _context.SaveChanges();

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

            return _context.SaveChanges();
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
    }
}
