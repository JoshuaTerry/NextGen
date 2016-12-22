using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DDI.Data.Models;
using DDI.Data.Models.Client;

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
        where T : class, IEntity
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

        public T GetById(Guid id) => EntitySet.Find(id);

        public T GetById(Guid id, params Func<T, string>[] includes)
        {
            T entity = EntitySet.Find(id);
            DbEntityEntry<T> dbEntry = _context.Entry(entity);

            foreach (Func<T, string> include in includes)
            {
                dbEntry.Reference(include(entity)).Load();
                //dbEntry.Collection()
                dbEntry.Collection(include(entity)).Load();
                //dbEntry.Member()
                //dbEntry.ComplexProperty()
                //dbEntry.Collection()
                //dbEntry.Reference(include).Load();
            }

            return dbEntry.Entity;
        }

        public T GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            //try
            //{
            //DbQuery<T> query = _context.Set<T>();
            //    string includePath = string.Join(".", includes.Select(i => NameFor(i)));

            //    return _context.Set<T>()
            //            .Include(includePath)
            //            .First(entity => entity.Id == id);
            //}
            //catch(Exception exception)
            //{
            //    Console.WriteLine(exception);

            //}
            T entity = EntitySet.Find(id);
            Type type = typeof(T);
            DbEntityEntry<T> dbEntry = _context.Entry(entity);
            //return null;
            //query.Include(includePath).First(entity => entity.Id == id);

            try
            {
                foreach (Expression<Func<T, object>> include in includes)
                {
                    string property = NameFor(include, true);
                    var prop = type.GetProperty(property);

                    if (prop.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                    {
                        dbEntry.Collection(property).Load();
                    }

                    else if (!prop.PropertyType.IsValueType)
                    {
                        //dbEntry.Property(property)
                        dbEntry.Reference(property).Load();
                    }
                    //query.Include(property);
                    //dbEntry.Reference(property).Load();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
            }

            //query.Where(e => e.Id == id).Load();
            //return query.First(entity => entity.Id == id);

            return dbEntry.Entity;
        }

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
