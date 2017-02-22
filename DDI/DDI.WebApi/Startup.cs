using DDI.WebApi;
using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Cors;


[assembly: OwinStartup(typeof(Startup))]

namespace DDI.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);

            ConfigureAuth(app);

            // Set the CacheHelper's cache provider
            Shared.Caching.CacheHelper.CacheProvider = new Providers.HttpCacheProvider();
        }
    }
}
