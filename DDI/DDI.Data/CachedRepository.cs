using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace DDI.Data
{
    public class CachedRepository<T> : Repository<T> where T : class
    {
        private readonly string _cacheKey;
        private static readonly object _cacheItemLock = new object();
        public CachedRepository() :
            this(new DomainContext())
        {
        }
        
        public CachedRepository(DbContext context) : base(context)
        {
            var type = typeof(T);
            string _cacheKey = $"{type.Name}_Key";
        }
        public override IQueryable<T> Entities
        {
            get
            {
                if (HttpContext.Current.Cache[_cacheKey] == null)
                {
                    lock (_cacheItemLock)
                    {
                        if (HttpContext.Current.Cache[_cacheKey] == null)
                        {
                            var data = EntitySet.ToList().AsQueryable<T>();
                            HttpContext.Current.Cache.Insert(_cacheKey, data, null, Cache.NoAbsoluteExpiration, new TimeSpan(1, 0, 0));
                        }
                    }
                }
                return (IQueryable<T>)HttpContext.Current.Cache[_cacheKey];
            }
        }

        public override void Delete(T entity)
        {
            HttpContext.Current.Cache[_cacheKey] = null;
            base.Delete(entity);
        }

        public override T Insert(T entity)
        {
            HttpContext.Current.Cache[_cacheKey] = null;
            return base.Insert(entity);
        }

        public override T Update(T entity)
        {
            HttpContext.Current.Cache[_cacheKey] = null;
            return base.Update(entity);
        }

        public override int UpdateChangedProperties(Guid id, IDictionary<string, object> propertyValues)
        {
            HttpContext.Current.Cache[_cacheKey] = null;
            return base.UpdateChangedProperties(id, propertyValues);
        }

        public override int UpdateChangedProperties(T entity, IDictionary<string, object> propertyValues)
        {
            HttpContext.Current.Cache[_cacheKey] = null;
            return base.UpdateChangedProperties(entity, propertyValues);
        }
    }
}