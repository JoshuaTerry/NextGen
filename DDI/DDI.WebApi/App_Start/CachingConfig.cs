using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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