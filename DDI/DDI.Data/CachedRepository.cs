using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DDI.Shared.Caching;

namespace DDI.Data
{
    public class CachedRepository<T> : Repository<T> where T : class
    {
        private readonly string _cacheKey = string.Empty;
        private static readonly object _cacheItemLock = new object();
        private const int DEFAULT_TIMEOUT_SECONDS = 1800; // 30 minutes

        private int _timeoutSeconds;
        private bool _isSlidingTimeout;

        public CachedRepository() :
            this(new DomainContext(), DEFAULT_TIMEOUT_SECONDS, false)
        {
        }

        public CachedRepository(DbContext context) :
            this(context, DEFAULT_TIMEOUT_SECONDS, false)
        {
        }

        public CachedRepository(DbContext context, int timeoutSeconds, bool isSlidingTimeout) : base(context)
        {
            var type = typeof(T);
            _cacheKey = $"{type.Name}_RepoKey";
            _timeoutSeconds = timeoutSeconds;
            _isSlidingTimeout = isSlidingTimeout;
        }

        public override IQueryable<T> Entities
        {
            get
            {
                return CacheHelper.GetEntry(_cacheKey, _timeoutSeconds, _isSlidingTimeout, () => EntitySet.ToList().AsQueryable(), null);
            }
        }

        public override void Delete(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            base.Delete(entity);
        }

        public override T Insert(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return base.Insert(entity);
        }

        public override T Update(T entity)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            return base.Update(entity);
        }

        public override void UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            base.UpdateChangedProperties(id, propertyValues, action);
        }

        public override void UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues, Action<T> action = null)
        {
            CacheHelper.RemoveEntry(_cacheKey);
            base.UpdateChangedProperties(entity, propertyValues, action);
        }
    }
}