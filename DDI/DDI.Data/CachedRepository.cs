using DDI.Shared.Caching;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DDI.Shared;
using System.Linq.Expressions;
using DDI.Shared.Models;

namespace DDI.Data
{
    public class CachedRepository<T> : IRepository<T> where T : class
    {
        private readonly string _cacheKey = string.Empty;
        private static readonly object _cacheItemLock = new object();
        private const int DEFAULT_TIMEOUT_SECONDS = 1800; // 30 minutes

        private int _timeoutSeconds;
        private bool _isSlidingTimeout;
        private IRepository<T> _baseRepository;
        private Expression<Func<T, object>>[] _includes { get; set; }

        public CachedRepository() :
            this(new Repository<T>(), null, DEFAULT_TIMEOUT_SECONDS, false)
        {
        }

        public CachedRepository(IRepository<T> repository) :
            this(repository, null, DEFAULT_TIMEOUT_SECONDS, false)
        {
        }

        public CachedRepository(IRepository<T> repository, params Expression<Func<T, object>>[] includes) :
            this(repository, includes, DEFAULT_TIMEOUT_SECONDS, false)
        {
        }

        public CachedRepository(IRepository<T> repository, Expression<Func<T, object>>[] includes, int timeoutSeconds, bool isSlidingTimeout)
        {
            var type = typeof(T);
            _cacheKey = $"{type.Name}_RepoKey";
            _timeoutSeconds = timeoutSeconds;
            _isSlidingTimeout = isSlidingTimeout;
            _baseRepository = repository;
            _includes = includes;
        }

        public void InvalidateCache()
        {
            CacheHelper.RemoveEntry(_cacheKey);
        }

        public IQueryable<T> Entities
        {
            get
            {
                return CacheHelper.GetEntry(_cacheKey, _timeoutSeconds, _isSlidingTimeout, () => _baseRepository.GetEntities(_includes).ToList().AsQueryable(), null);
            }
            private set
            {
                CacheHelper.SetEntry(_cacheKey, _baseRepository.Entities.ToList().AsQueryable(), _timeoutSeconds, _isSlidingTimeout, null);
            }
        }

        public ISQLUtilities Utilities
        {
            get
            {
                return _baseRepository.Utilities;
            }
        }

        public void Delete(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            _baseRepository.Delete(entity);
        }

        public T Insert(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return _baseRepository.Insert(entity);
        }

        public T Update(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return _baseRepository.Update(entity);
        }

        public void UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            _baseRepository.UpdateChangedProperties(id, propertyValues, action);
        }

        public void UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            _baseRepository.UpdateChangedProperties(entity, propertyValues, action);
        }

        public Shared.EntityState GetEntityState(T entity)
        {
            return _baseRepository.GetEntityState(entity);
        }

        public T GetById(Guid id)
        {
            return Entities.Cast<ICanTransmogrify>().FirstOrDefault(p => p.Id == id) as T;
        }
        
        public T GetById(Guid id, params Expression<Func<T, object>>[] includes)
        {
            return _baseRepository.GetById(id, includes);
        }

        public T Find(params object[] keyValues)
        {
            return _baseRepository.Find(keyValues);
        }

        public T Create()
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return _baseRepository.Create();
        }

        public List<string> GetModifiedProperties(T entity)
        {
            return _baseRepository.GetModifiedProperties(entity);
        }

        public void LoadReference<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collection) where TElement : class
        {
            _baseRepository.LoadReference<TElement>(entity, collection);
        }

        public void LoadReference<TElement>(T entity, Expression<Func<T, TElement>> property) where TElement : class
        {
            _baseRepository.LoadReference<TElement>(entity, property);
        }

        public ICollection<TElement> GetReference<TElement>(T entity, Expression<Func<T, ICollection<TElement>>> collection) where TElement : class
        {
            return _baseRepository.GetReference<TElement>(entity, collection);
        }

        public TElement GetReference<TElement>(T entity, Expression<Func<T, TElement>> property) where TElement : class
        {
            return _baseRepository.GetReference<TElement>(entity, property);
        }

        public ICollection<T> GetLocal()
        {
            return _baseRepository.GetLocal();
        }

        public T Attach(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return _baseRepository.Attach(entity);
        }

        public IQueryable<T> GetEntities(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> entities = _baseRepository.GetEntities(includes);
            Entities = entities;
            return entities;
        }
    }
}