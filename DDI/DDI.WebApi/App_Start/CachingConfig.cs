namespace DDI.WebApi
{
    public class CachingConfig
    {
        public static void Configure()
        {
            // Set the CacheHelper's cache provider
            Shared.Caching.CacheHelper.CacheProvider = new Providers.HttpCacheProvider();
        }
    }
}