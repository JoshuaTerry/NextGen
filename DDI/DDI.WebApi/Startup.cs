using DDI.Services;
using DDI.Services.Search;
using DDI.WebApi;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DDI.WebApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Set the CacheHelper's cache provider
            Shared.Caching.CacheHelper.CacheProvider = new Providers.HttpCacheProvider();

            //var cs = new ConstituentService();
            //var result = cs.GetAll(new ConstituentSearch() { Name = "Hammer", Limit = 10, Offset = 0 });
        }
    }
}
