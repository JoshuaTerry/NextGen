using System;
using System.Runtime.Caching;

namespace DDI.Shared.Caching
{
    /// <summary>
    /// Cache provider that uses System.Runtime.Caching.MemoryCache
    /// </summary>
    internal class MemoryCacheProvider : ICacheProvider
    {
        private ObjectCache _cache;
         
        public MemoryCacheProvider()
        {
            _cache = System.Runtime.Caching.MemoryCache.Default;
        }

        public void RemoveEntry(string key)
        {
            _cache.Remove(key);
        }

        public T GetEntry<T>(string key, int timeoutSecs, bool isSlidingTimeout, Func<T> entryProvider, Action<T> callback) where T : class
        {

            T entry = _cache.Get(key) as T;            
            if (entry == null)
            {
                entry = entryProvider.Invoke();
                SetEntry(key, entry, timeoutSecs, isSlidingTimeout, callback);
            }

            return entry;

        }

        public void SetEntry<T>(string key, T entry, int _timeoutSecs, bool isSlidingTimeout, Action<T> callback) where T : class
        {

            CacheItemPolicy policy = new CacheItemPolicy()
            {
                AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration,
                SlidingExpiration = ObjectCache.NoSlidingExpiration
            };

            if (isSlidingTimeout)
            {
                policy.SlidingExpiration = TimeSpan.FromSeconds(_timeoutSecs);
            }
            else if (_timeoutSecs > 0)
            {
                policy.AbsoluteExpiration = DateTime.Now.AddSeconds(_timeoutSecs);
            }

            if (callback != null)
            {
                policy.RemovedCallback = args => callback.Invoke(args.CacheItem.Value as T);
            }

            _cache.Set(key, entry, policy);

        }

    }
}
