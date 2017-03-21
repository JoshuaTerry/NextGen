using System;

namespace DDI.Shared.Caching
{
    /// <summary>
    /// Helper class for providing adding and retrieving objects from a central cache provider.
    /// </summary>
    public static class CacheHelper
    {
        private static ICacheProvider _cacheProvider;

        public static ICacheProvider CacheProvider
        {
            get
            {
                if (_cacheProvider == null)
                {
                    _cacheProvider = new MemoryCacheProvider(); // This is the default if no provider has been specified.
                }
                return _cacheProvider;
            }
            set
            {
                _cacheProvider = value;
            }
        }

        /// <summary>
        /// Get an entry from the cache.  If the entry has not been added, the entryProvider function is called and the entry is added to the cache. 
        /// </summary>
        public static T GetEntry<T>(string key, Func<T> entryProvider) where T : class
        {
            return CacheProvider.GetEntry<T>(key, 0, false, entryProvider, null);
        }

        /// <summary>
        /// Get an entry from the cache.  If the entry has not been added, the entryProvider function is called and the entry is added to the cache. 
        /// </summary>
        public static T GetEntry<T>(string key, int timeout, bool isSlidingTimeout, Func<T> entryProvider, Action<T> callback) where T : class
        {
            return CacheProvider.GetEntry<T>(key, timeout, isSlidingTimeout, entryProvider, callback);
        }

        /// <summary>
        /// Add an entry to the cache, or update it if it has already been added.
        /// </summary>
        public static void SetEntry<T>(string key, T entry) where T : class
        {
            CacheProvider.SetEntry<T>(key, entry, 0, false, null);
        }

        /// <summary>
        /// Add an entry to the cache, or update it if it has already been added.
        /// </summary>
        public static void SetEntry<T>(string key, T entry, int timeout, bool isSlidingTimeout, Action<T> callback) where T : class
        {
             CacheProvider.SetEntry<T>(key, entry, timeout, isSlidingTimeout, callback);
        }

        /// <summary>
        /// Remove an entry from the cache, or update it if it has already been added.
        /// </summary>
        public static void RemoveEntry(string key)
        {
            CacheProvider.RemoveEntry(key);
        }

    }
}
