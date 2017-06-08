using DDI.Shared.Caching;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

namespace DDI.WebApi.Providers
{
    /// <summary>
    /// Cache provider that uses  HttpContext.Current.Cache.
    /// </summary>
    public class HttpCacheProvider : ICacheProvider
    {
        /// Dictionary for cache item expiration callbacks (since web cache implements this a bit differently then runtime caching.)
        private Dictionary<string, Action> _entryCallbacks = new Dictionary<string, Action>();

        // Cache lock object
        private static readonly object _cacheItemLock = new object();

        private Cache _currentCache = null;
        private Cache CurrentCache => _currentCache ?? (_currentCache = _currentCache = HttpContext.Current.Cache);

        public void RemoveEntry(string key)
        {
            CurrentCache.Remove(key);
        }

        public T GetEntry<T>(string key, int timeoutSeconds, bool isSlidingTimeout, Func<T> entryProvider, Action<T> callback) where T : class
        {
            if (CurrentCache[key] == null)
            {
                lock (_cacheItemLock)
                {
                    if (CurrentCache[key] == null)
                    {
                        T entry = entryProvider.Invoke();
                        SetEntry(key, entry, timeoutSeconds, isSlidingTimeout, callback);
                    }
                }
            }
            return CurrentCache[key] as T;
        }

        public void SetEntry<T>(string key, T entry, int _timeoutSecs, bool isSlidingTimeout, Action<T> callback) where T : class
        {
            DateTime absoluteExpiration = Cache.NoAbsoluteExpiration;
            TimeSpan slidingExpiration = Cache.NoSlidingExpiration;

            if (isSlidingTimeout)
            {
                slidingExpiration = TimeSpan.FromSeconds(_timeoutSecs);
            }
            else if (_timeoutSecs > 0)
            {
                absoluteExpiration = DateTime.Now.AddSeconds(_timeoutSecs);
            }

            if (callback == null)
            {
                CurrentCache.Insert(key, entry, null, absoluteExpiration, slidingExpiration);
            }
            else
            {
                _entryCallbacks[key] = () => callback(entry);
                CurrentCache.Insert(key, entry, null, absoluteExpiration, slidingExpiration, Callback);
            }

        }

        private void Callback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration)
        {
            // Retrieve the callback action from the dictionary, then invoke it.
            Action callbackAction;
            if (_entryCallbacks.TryGetValue(key, out callbackAction) && callbackAction != null)
            {
                callbackAction.Invoke();
            }

            // The callback doesn't attempt to provide an updated entry.
            expensiveObject = null;
            dependency = null;
            absoluteExpiration = Cache.NoAbsoluteExpiration;
            slidingExpiration = Cache.NoSlidingExpiration;
        }

    }
}