using DDI.Shared;
using DDI.Shared.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DDI.Data
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T : class, IReadOnlyEntity 
    {
        #region Private Fields

        private readonly DbContext _context = null;
        private IDbSet<T> _entities = null;
        private bool _isUOW = false;
        private ICollection<T> _local = null;

        #endregion Private Fields

        #region Public Properties

        public virtual IQueryable<T> Entities => EntitySet;
          
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

        public ReadOnlyRepository() :
            this(new DomainContext())
        {
            _isUOW = false;
        }

        public ReadOnlyRepository(DbContext context)
        {
            _context = context;
            _isUOW = (context != null);
        }
        #endregion Public Constructors

        #region Public Methods

        public static string NameFor<T1>(Expression<Func<T1, object>> property, bool shouldContainObjectPath = false)
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
                var objectPath = member.Expression.ToString().Split('.').Where(a => !a.Equals("First()")).ToArray();
                if (objectPath.Length >= 2)
                {
                    path = String.Join(".", objectPath, 1, objectPath.Length - 1);
                    return $"{path}.{member.Member.Name}";
                }
            }
            return member?.Member.Name ?? String.Empty;
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
